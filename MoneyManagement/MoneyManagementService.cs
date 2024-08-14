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


        public Task<Account> LoadAccount(Guid id)
        {
            return _accountRepository.LoadAccount(id);
        }

        public Task SaveAccount(Account account)
        {
            return _accountRepository.SaveAccount(account);
        }


        public Task<List<Transaction>> LoadTransactions(Guid accountId)
        {
            return _transactionRepository.LoadTransactions(accountId);
        }

        
        public async Task<string> SaveTransactions(List<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions) 
            {
                var account = await LoadAccount(transaction.AccountId);
                //var notValid = account.TransactionNotValid(transaction);
                var valid = account.TransactionValid(transaction);

                if (!valid)
                {
                    return "Transfer not possible";
                }
            }
            return await _transactionRepository.SaveTransactions(transactions);
        }


        //hier ist async & await wichtig, da ich eine Operation mit categories anfange
        public Task<List<Category>> GetCategories()
        {
            return _categoryRepository.GetCategories();
        }



        //Factory-Methode: das wir nur für die Konsolenanwendung gebraucht, damit ich nicht ständig das Objekt kompliziert erzeugen muss, sondern nur MoneyManagementService.Create()
        //bei Websanwendungen kann ich dann dieses MoneyManagementDing per DI Container injizieren in der Program.cs
        public static MoneyManagementService Create()
        {
            var financecontext = new FinanceContext();
            return new(new AccountRepository(financecontext), new TransactionRepository(financecontext), new CategoryRepository(financecontext));
        }
    }
}
