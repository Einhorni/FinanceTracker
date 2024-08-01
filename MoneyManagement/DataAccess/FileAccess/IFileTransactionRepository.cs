using MoneyManagement.Models;

namespace MoneyManagement.DataAccess.FileAccess
{
    public interface IFileTransactionRepository
    {
        public List<TransactionDTO> LoadTransactions();
        public void SaveTransaction(TransactionDTO transaction);
    }
}
