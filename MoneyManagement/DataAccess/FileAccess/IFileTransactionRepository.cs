using MoneyManagement.Models;

namespace MoneyManagement.DataAccess.FileAccess
{
    public interface IFileTransactionRepository
    {
        public List<Transaction> LoadTransactions();
        public void SaveTransaction(Transaction transaction);
    }
}
