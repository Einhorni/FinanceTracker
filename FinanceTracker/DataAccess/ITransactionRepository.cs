using FinanceTracker.MoneyManagement;

namespace FinanceTracker.DataAccess
{
    public interface ITransactionRepository
    {
        public List<Transaction> LoadTransactions();
        public void SaveTransaction(Transaction transaction);
    }
}
