using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyManagement.BusinessModels;
using MoneyManagement.DbContexts;
using MoneyManagement.Entities;
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
            var transactionEntities = await
                _financeContext.Transactions
                .AsNoTracking()
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            var transactions =
                transactionEntities
                .Select(t => t.TransactionEntityToTransaction())
                .ToList();

            return transactions;
        }


        public async Task SaveTransactions(List<Transaction> transactions)
        {
            ArgumentNullException.ThrowIfNull(transactions);
            // =
            //if (transactions is null) throw new ArgumentNullException(nameof(transactions));

            var transactionEntities =
                transactions
                    .Select(t => t.TransactionToTransactionEntity())
                    .ToList();

            await _financeContext.Transactions.AddRangeAsync(transactionEntities);
            await _financeContext.SaveChangesAsync();
        }
    }

}
