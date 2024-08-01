using MoneyManagement.Models;
using System.Transactions;

namespace MoneyManagement.DataAccess
{
    internal interface ITransactionRepository
    {
        Task<decimal> GetBalance(Guid accountId);
        Task <List<TransactionDTO>> LoadTransactions(Guid accountId);
    }
}