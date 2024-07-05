using MoneyManagement.Models;

namespace FinanceTracker.DataAccess
{
    public interface IAccountRepository
    {
        public List<Account> LoadAccounts();
        public void SaveAccounts(List<Account> accounts);
    }
}
