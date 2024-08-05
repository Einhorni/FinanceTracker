using MoneyManagement.Models;

namespace MoneyManagement.DataAccess.FileAccess
{
    public interface IFileAccountRepository
    {
        public List<Account> LoadAccounts();
        public void SaveAccounts(List<Account> accounts);
    }
}
