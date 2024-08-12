using Microsoft.EntityFrameworkCore;
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
            var categories = await _financeContext.Categories./*Where(c => c.Expense).*/ToListAsync();

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


        public async Task<string> SaveTransactions(List<Transaction> transactions)
        {
            var transactionEntitiesFromDB = await _financeContext.Transactions.ToListAsync();

            //Prüfung, ob negative Transaction valide
            var negativeTransaction = transactions
                .Where(t => t.Amount < 0).First();

            var account = await _financeContext.Accounts
                .Where(a => a.Id == negativeTransaction.AccountId).FirstAsync();

            var accountBalance = transactionEntitiesFromDB
                .Where(t => t.AccountId == account.Id)
                .Select(t => t.Amount)
                .Sum();

            if (account.KindOfAccount == "Bargeldkonto" && accountBalance < negativeTransaction.Amount ||
                account.KindOfAccount == "Girokonto" && (accountBalance + account.Overdraft) < negativeTransaction.Amount)
                return "Transfer not possible";
            
            //Durchführung Transfer
            else
            {
                var transactionEntities = new List<TransactionEntity>();

                foreach (var transaction in transactions)
                {
                    var transactionEntity = transaction.TransactionToTransactionEntity();
                    transactionEntities.Add(transactionEntity);
                }

                await _financeContext.Transactions.AddRangeAsync(transactionEntities);
                await _financeContext.SaveChangesAsync();

                return "Transfer saved";
            }
        }
    }

}
