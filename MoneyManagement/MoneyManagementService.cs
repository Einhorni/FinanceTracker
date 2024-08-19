using MoneyManagement.DataAccess.FileAccess;
using MoneyManagement.Models;
using MoneyManagement.DataAccess;
using MoneyManagement.Entities;
using MoneyManagement.DbContexts;
using CSharpFunctionalExtensions;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using Serilog;

namespace MoneyManagement
{
    public class MoneyManagementService
    {

        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _logger;

        public MoneyManagementService(IAccountRepository accountRepository, ITransactionRepository transactionRepository, ICategoryRepository categoryRepository, ILogger logger)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _logger = logger;
        }



        public Task<List<Account>> LoadAccounts()
        {
            return _accountRepository.LoadAccounts();
        }

        public Task<Maybe<Account>> LoadAccount(Guid id)
        {
            var account = _accountRepository.LoadAccount(id);
            return account;
        }

        public Task SaveAccount(Account account)
        {
            //keine Nullprüfung, weil nur Wrapper, aber möglich, wenn ich das Repo nicht kenne
            return _accountRepository.SaveAccount(account);
        }


        public Task<List<Transaction>>LoadTransactions(Guid accountId)
        {
            return _transactionRepository.LoadTransactions(accountId);
        }
        
        // Nuget: CSharpFunctionalExtensions  (Result-Type)
        // Businessprüfung: Fehlermeldung in UI (Validation)
        public async Task<Result> SaveTransactions(List<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                var account = await LoadAccount(transaction.AccountId);
                
                if (!account.HasValue)
                    throw new AccountNotFoundException (nameof(account));
                
                var valid = account.Value.TransactionValid(transaction);

                if (!valid)
                    return Result.Failure("Not enough money available.");
            }
            
            await _transactionRepository.SaveTransactions(transactions);
            return Result.Success();
        }


        public Task<List<Category>> GetCategories()
        {
            return _categoryRepository.GetCategories();
        }



        //Factory-Methode: das wir nur für die Konsolenanwendung gebraucht, damit ich nicht ständig das Objekt kompliziert erzeugen muss, sondern nur MoneyManagementService.Create()
        //bei Websanwendungen kann ich dann dieses MoneyManagementDing per DI Container injizieren in der Program.cs
        //Encapsulating object creation == decoupling, open closed principle
        //central point of control -> testing and mocking
        public static MoneyManagementService Create(ILogger serilog)
        {
            var financecontext = new FinanceContext();
            return new(new AccountRepository(financecontext), new TransactionRepository(financecontext), new CategoryRepository(financecontext), serilog);
        }
    }

    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(string message) : base(message) { }
    }
}
