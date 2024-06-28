using FinanceTracker.Classes;

namespace FinanceTracker.DataAccess
{
    internal interface ITransactionInterface
    {
        public List<Transaction> LoadTransactions();
        public void SaveTransactions(List<Transaction> transactions);
    }
}
