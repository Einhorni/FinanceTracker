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


        public Task SaveAccount(Account account)
        {
            return _accountRepository.SaveAccount(account);
        }

        
        public Task<List<Transaction>> LoadTransactions(Guid accountId)
        {
            return _transactionRepository.LoadTransactions(accountId);
        }

        //kein async (ist möglich, aber...), nur ein Task, weil nichts weiter mit dem Ergebnis gemacht wird -> avoid state machine (creates overhead)
        //auf den Task wird dann in der Oberfläche mit Wait() (wenn void zurückkommt) oder Result (wenn es eine Rückgabe gibt) gewartet
        public Task SaveTransactions(List<Transaction> transactions)
        {
            return _transactionRepository.SaveTransactions(transactions);
        }

        //hier ist async & await wichtig, da ich eine Operation mit categories anfange
        public Task<List<Category>> GetCategories()
        {
            return _categoryRepository.GetCategories();
            //var categoriesList = categories.Select(c => c.Name).ToList();
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
