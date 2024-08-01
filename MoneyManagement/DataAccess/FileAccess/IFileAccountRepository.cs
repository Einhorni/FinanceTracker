using MoneyManagement.Models;

namespace MoneyManagement.DataAccess.FileAccess
{
    public interface IFileAccountRepository
    {
        public List<AccountDTO> LoadAccounts();
        public void SaveAccounts(List<AccountDTO> accounts);
    }
}
