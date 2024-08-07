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
using Transaction = MoneyManagement.Models.Transaction;

namespace MoneyManagement.DataAccess
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly FinanceContext _financeContext;

        public TransactionRepository(FinanceContext financeContext)
        {
            _financeContext = financeContext ?? throw new ArgumentNullException(nameof(financeContext));
        }

        public async Task<List<Transaction>> LoadTransactions(Guid accountId)
        {
            var categories = await _financeContext.Categories.Where(c => c.Expense).ToListAsync();

            var transactionEntities = await
                _financeContext.Transactions
                .Where(t => t.AccountId == accountId)
                .Where(t => categories
                    .Select(c=> c.Name)
                    .Contains(t.CategoryName))
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            var transactions =
                transactionEntities
                .Select(t => t.TransactionEntityToTransaction())
                .ToList();

            return transactions;
        }

        public async Task<decimal> GetBalance(Guid accountId)
        {
            var transactions = await
            _financeContext.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
            return transactions.Sum(t => t.Amount);
        }

        public async Task SaveTransactions(List<Transaction> transactions)
        {
            var transactionEntities = new List<TransactionEntity>();

            foreach (var transaction in transactions)
            {
                var transactionEntity = transaction.TransactionToTransactionEntity();
                //var transaction = Mappings.TransactionToTransactionEntity(transactionDTO);
                transactionEntities.Add(transactionEntity);
            }

            await _financeContext.Transactions.AddRangeAsync(transactionEntities);
            await _financeContext.SaveChangesAsync();
        }
    }

}
