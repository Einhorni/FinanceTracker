using MoneyManagement.DataAccess.FileAccess;
using MoneyManagement.Models;
using MoneyManagement.DataAccess;
using MoneyManagement.Entities;
using MoneyManagement.DbContexts;

namespace MoneyManagement
{
    public class MoneyManagementService
    {

        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;

        public MoneyManagementService(IAccountRepository accountRepository, ITransactionRepository transactionRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }



        public Task<List<Account>> LoadAccounts()
        {
            return _accountRepository.LoadAccounts();
        }

        public async Task<Account> LoadAccount(Guid id)
        {
            var account = await _accountRepository.LoadAccount(id);
            //keine ex, sondern account oder null zurückgeben --> erwarteter Fehler
            //oder resulttype mit string als fehlerobjekt
            if (account is null)
                throw new ArgumentNullException("No account found");
            return account;
        }

        public Task SaveAccount(Account account)
        {
            //keine Nullprüfung, weil nur Wrapper, aber möglich, wenn ich das Repo nicht kenne
            return _accountRepository.SaveAccount(account);
        }


        public Task<List<Transaction>> LoadTransactions(Guid accountId)
        {
            return _transactionRepository.LoadTransactions(accountId);
        }

        // CodeReview: ggf. ResultType anlegen, 2 properties. Success: bool, ErrorMessage: string
        // Nuget: CSharpFunctionalExtensions  (Result-Type)
        // zurückgeben und fehlermeldung darauf basierend ausgeben - es ist eine businessprüfung
        // andere prüfungen können direkt im client gemacht werden (tranasktion kleiner 0 oder sowas)
        public async Task SaveTransactions(List<Transaction> transactions)
        {
            //null prüfung
            foreach (Transaction transaction in transactions) 
            {
                var account = await LoadAccount(transaction.AccountId);
                var valid = account.TransactionValid(transaction);

                //keine erwarteten Fehler mit exceptions -> in UI
                if (!valid)
                {
                    throw new InvalidOperationException("Not enough money on account");
                }
            }
            await _transactionRepository.SaveTransactions(transactions);
        }


        public Task<List<Category>> GetCategories()
        {
            return _categoryRepository.GetCategories();
        }



        //Factory-Methode: das wir nur für die Konsolenanwendung gebraucht, damit ich nicht ständig das Objekt kompliziert erzeugen muss, sondern nur MoneyManagementService.Create()
        //bei Websanwendungen kann ich dann dieses MoneyManagementDing per DI Container injizieren in der Program.cs
        //Encapsulating object creation == decoupling, open closed principle
        //central point of control -> testing and mocking
        public static MoneyManagementService Create()
        {
            var financecontext = new FinanceContext();
            return new(new AccountRepository(financecontext), new TransactionRepository(financecontext), new CategoryRepository(financecontext));
        }
    }
}
