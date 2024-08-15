using MoneyManagement.Models;
using MoneyManagement.Entities;

namespace MoneyManagement.DataAccess
{
    public interface ITransactionRepository
    {
        Task <List<Transaction>> LoadTransactions(Guid accountId);
        Task SaveTransactions(List<Transaction> transactions);
    }
}