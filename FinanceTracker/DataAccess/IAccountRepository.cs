using FinanceTracker.MoneyManagement;

namespace FinanceTracker.DataAccess
{
    public interface IAccountRepository
    {
        public List<Account> LoadAccounts();
        public void SaveAccounts(List<Account> accounts);
        public Account LoadAccountById(Guid id);
        //public void SaveAccountById(Account account);
    }
}
