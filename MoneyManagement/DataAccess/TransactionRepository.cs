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
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            var transactions =
                transactionEntities
                .Select(t => t.TransactionEntityToTransaction())
                .ToList();

            return transactions;
        }

        // CodeReview: return nur Task, Und Statusmeldungen den überliegenden Schichten überlassen - nicht im Service -> in UI oder später in der WebAPI mit StatusCodes
        //Wenn ich in einer tieferliegenden Schicht (Backend) eine Ex werfe, dann an der entsprechenden Stelle In UI bei Aufruf der Funktion try catch
        public async Task SaveTransactions(List<Transaction> transactions)
        {
            if (transactions.IsNullOrEmpty()) return; //teilen in null dann throw NullArgument return und empty 

            var transactionEntities = new List<TransactionEntity>();

            transactionEntities
                .AddRange(transactions
                    .Select(t => t.TransactionToTransactionEntity()));

            await _financeContext.Transactions.AddRangeAsync(transactionEntities);
            await _financeContext.SaveChangesAsync();
        }
    }

}
