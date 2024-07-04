using FinanceTracker.MoneyManagement;
using FinanceTracker.DataAccess;

namespace FinanceTracker.MoneyManagement
{
    public class AccountManager
    {
        private readonly IAccountRepository _accountRepository;
        public List<Account> Accounts;

        public AccountManager(IAccountRepository accountRepository) //, Guid optional = new Guid())  --> if I wanted to use an optional Guid parameter
        {
            //var guidIsEmpty = optional == Guid.Empty;

            _accountRepository = accountRepository;
            Accounts = LoadAccounts();

            //if (!guidIsEmpty)
            //{
            //    Account = LoadAccountById(optional);
            //}
        }

        
        public List<Account> LoadAccounts() 
        { return _accountRepository.LoadAccounts(); }

        public void SaveAccounts (List<Account> accounts)
        { _accountRepository.SaveAccounts(accounts);}
    }
}
