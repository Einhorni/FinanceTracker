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
using Transaction = MoneyManagement.Entities.Transaction;

namespace MoneyManagement.DataAccess
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly FinanceContext _financeContext;

        public TransactionRepository(FinanceContext financeContext)
        {
            _financeContext = financeContext ?? throw new ArgumentNullException(nameof(financeContext));
        }

        public async Task<List<Entities.Transaction>> LoadTransactions(Guid accountId)
        {
            var transactions = await
                _financeContext.Transactions
                .Where(t => t.AccountId == accountId.ToString())
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            return transactions;
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

        public async Task SaveTransactions(List<Transaction> transactions)
        {
            var loadedTransactions = await _financeContext.Transactions.ToListAsync();
            loadedTransactions.AddRange(transactions);
            await _financeContext.SaveChangesAsync();
        }
    }

}
