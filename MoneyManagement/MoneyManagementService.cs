using MoneyManagement.DataAccess.FileAccess;
using MoneyManagement.Models;
using MoneyManagement.DataAccess;
using MoneyManagement.Entities;

namespace MoneyManagement
{
    public class MoneyManagementService
    {
        private readonly IAccount? _account;
        private readonly IAccountRepository? _accountRepository;
        public Task<List<AccountDTO>>? Accounts;
        private readonly ITransactionRepository? _transactionRepository;
        public Task<List<TransactionDTO>>? Transactions;
        private readonly ICategoryRepository? _categoryRepository;


        public MoneyManagementService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public MoneyManagementService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            //Accounts = LoadAccounts();
        }

        public MoneyManagementService(ITransactionRepository transactionRepository) //, Guid accountId)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            //Transactions = LoadTransactions(accountId);
        }

        public MoneyManagementService(IAccount account, IAccountRepository accountRepository, ITransactionRepository transactionRepository, Guid accountId)
        {
            _account = account;
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            //Accounts = LoadAccounts();
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            //Transactions = LoadTransactions(accountId);
        }



        public decimal AddAmount(decimal amount)
        { return _account.AddAmount(amount); }
        public decimal SubstractAmount(decimal amount)
        { return _account.SubstractAmount(amount); }



        public async Task<List<AccountDTO>> LoadAccounts()
        {
            var accounts = await _accountRepository.LoadAccounts();

            var accountDTOs = new List<AccountDTO>();

            foreach (var account in accounts) 
            {
                
                var balance = await _transactionRepository.GetBalance(Guid.Parse(account.Id));
                var accountDto = Mappings.AccountToAccountDto(account, balance);
                accountDTOs.Add(accountDto);
                   
            }
            
            return accountDTOs ; 
        }


        public void SaveAccount(AccountDTO accountDto)
        {
            var account = Mappings.AccountDtoToAccount(accountDto);

            _accountRepository.SaveAccount(account); 
        }


        public async Task<List<TransactionDTO>> LoadTransactions(Guid accountId)
        {
            var transactionDTOs = new List<TransactionDTO>();
            var transactions = await _transactionRepository.LoadTransactions(accountId);
            foreach (var transaction in transactions)
            {
                var transactionDTO =
                    new TransactionDTO
                    {
                        TransactionId = Guid.Parse(transaction.Id),
                        AccountId = Guid.Parse(transaction.AccountId),
                        Amount = transaction.Amount,
                        FromAccountId = Guid.Parse(transaction.FromAccountId),
                        ToAccountId = Guid.Parse(transaction.ToAccountId),
                        Category = transaction.Category,
                        Date = transaction.Date,
                        Title = transaction.Title
                    };
                transactionDTOs.Add(transactionDTO);
            }

            return transactionDTOs;
                
        }


        public void SaveTransactions(List<TransactionDTO> transactionDTOs)
        {
            var transactions = new List<Transaction>();

            foreach (var transactionDTO in transactionDTOs)
            {
                var transaction = Mappings.TransactionDtoToTransaction(transactionDTO);
                transactions.Add(transaction);
            }

            _transactionRepository.SaveTransactions(transactions);
        }


        public async Task<List<string>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategories();
            var categoriesList = categories.Select(c => c.Name).ToList();
            return categoriesList;
        }
    }
}
