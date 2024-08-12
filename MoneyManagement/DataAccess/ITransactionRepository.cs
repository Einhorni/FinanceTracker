using MoneyManagement.Models;
using MoneyManagement.Entities;

namespace MoneyManagement.DataAccess
{
    public interface ITransactionRepository
    {
        Task <List<Transaction>> LoadTransactions(Guid accountId);
        Task <string> SaveTransactions(List<Transaction> transactions);
    }
}