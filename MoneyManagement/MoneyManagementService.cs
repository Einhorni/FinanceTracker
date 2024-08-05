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


        public void SaveAccount(Account account)
        {

            _accountRepository.SaveAccount(account);
        }


        public async Task<List<Transaction>> LoadTransactions(Guid accountId)
        {
            var transactions = await _transactionRepository.LoadTransactions(accountId);
            

            return transactions;

        }


        public void SaveTransactions(List<Transaction> transactions)
        {
            _transactionRepository.SaveTransactions(transactions);
        }


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
