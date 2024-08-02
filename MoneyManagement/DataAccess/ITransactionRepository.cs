using MoneyManagement.Models;
using MoneyManagement.Entities;

namespace MoneyManagement.DataAccess
{
    public interface ITransactionRepository
    {
        Task<decimal> GetBalance(Guid accountId);
        Task <List<Transaction>> LoadTransactions(Guid accountId);

        Task SaveTransactions(List<Transaction> transactions);
    }
}