using MoneyManagement.Models;
using MoneyManagement.Entities;
using CSharpFunctionalExtensions;

namespace MoneyManagement.DataAccess
{
    public interface ITransactionRepository
    {
        Task <List<Transaction>> LoadTransactions(Guid accountId);
        Task SaveTransactions(List<Transaction> transactions);
    }
}