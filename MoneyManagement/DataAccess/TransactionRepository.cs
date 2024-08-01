using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using MoneyManagement.DbContexts;
using MoneyManagement.Entities;
using MoneyManagement.Models;

namespace MoneyManagement.DataAccess
{
    internal class TransactionRepository : ITransactionRepository
    {
        private readonly FinanceContext _financeContext;

        public TransactionRepository(FinanceContext financeContext)
        {
            _financeContext = financeContext;
        }

        public async Task<List<TransactionDTO>> LoadTransactions(Guid accountId)
        {
            var transactions = await
                _financeContext.Transactions
                .Where(t => t.AccountId == accountId.ToString())
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            var transactionsDto =
                transactions
                .Select(t => new TransactionDTO
                {
                    TransactionId = Guid.Parse(t.Id),
                    AccountId = Guid.Parse(t.AccountId),
                    Amount = t.Amount,
                    FromAccountId = Guid.Parse(t.FromAccountId),
                    ToAccountId = Guid.Parse(t.ToAccountId),
                    Category = t.Category,
                    Date = t.Date,
                    Title = t.Title
                })
                .ToList();
            return transactionsDto;
        }

        public async Task<decimal> GetBalance(Guid accountId)
        {
            var transactions = await
            _financeContext.Transactions
                .Where(t => t.AccountId == accountId.ToString())
                .OrderByDescending(t => t.Date)
                .ToListAsync();
            return transactions.Sum(t => t.Amount);
        }
    }
}
