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



        public async Task<List<Account>> LoadAccounts()
        {
            var accounts = await _accountRepository.LoadAccounts();

            return accounts;
        }


        public Task SaveAccount(Account account)
        {

            return _accountRepository.SaveAccount(account);
        }

        //ist möglich das hier mit async & await zu machen, es geht aber auch mit Task & return wie oben und unten
        public async Task<List<Transaction>> LoadTransactions(Guid accountId)
        {
            var transactions = await _transactionRepository.LoadTransactions(accountId);
            return transactions;
        }


        public Task SaveTransactions(List<Transaction> transactions)
        {
            return _transactionRepository.SaveTransactions(transactions);
        }

        //hier ist async & await wichtig, da ich eine Operation mit categories anfange
        public async Task<List<string>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategories();
            var categoriesList = categories.Select(c => c.Name).ToList();
            return categoriesList;
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
