using MoneyManagement.DataAccess.FileAccess;
using MoneyManagement.Models;
using MoneyManagement.DataAccess;
using MoneyManagement.Entities;

namespace MoneyManagement
{
    internal class MoneyManagementService
    {
        private readonly IAccount? _account;
        private readonly IAccountRepository? _accountRepository;
        public Task<List<AccountDTO>>? Accounts;
        private readonly ITransactionRepository? _transactionRepository;
        public Task<List<TransactionDTO>>? Transactions;



        public MoneyManagementService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            Accounts = LoadAccounts();
        }

        public MoneyManagementService(ITransactionRepository transactionRepository, Guid accountId)
        {
            _transactionRepository = transactionRepository;
            Transactions = LoadTransactions(accountId);
        }

        public MoneyManagementService(IAccount account, IAccountRepository accountRepository, ITransactionRepository transactionRepository, Guid accountId)
        {
            _account = account;
            _accountRepository = accountRepository;
            Accounts = LoadAccounts();
            _transactionRepository = transactionRepository;
            Transactions = LoadTransactions(accountId);
        }


        public decimal AddAmount(decimal amount)
        { return _account.AddAmount(amount); }
        public decimal SubstractAmount(decimal amount)
        { return _account.SubstractAmount(amount); }


        public async Task<List<AccountDTO>> LoadAccounts()
        {
            //accounts laden
            var accounts = await _accountRepository.LoadAccounts();

            var accountDTOs = new List<AccountDTO>();

            //für jedes account die id nehmen und aus den jeweiligen transactions die summe errechnen
            foreach (var account in accounts) 
            {
                
                var balance = await _transactionRepository.GetBalance(Guid.Parse(account.Id));
                var accountDto = Mappings.AccountToAccountDto(account, balance);
                accountDTOs.Add(accountDto);
                   
            }
            
            return accountDTOs ; 
        }

        //public void SaveAccounts(List<AccountDTO> accounts)
        //{ _accountRepository.SaveAccounts(accounts); }

        public async Task<List<TransactionDTO>> LoadTransactions(Guid accountId)
        {
            return await _transactionRepository.LoadTransactions(accountId);
        }

        //public void SaveTransaction(TransactionDTO transaction)
        //{
        //    _transactionRepository.SaveTransaction(transaction);
        //}
    }
}
