using Microsoft.EntityFrameworkCore;
using MoneyManagement.DbContexts;
using MoneyManagement.Entities;
using MoneyManagement.Models;
using System.Security.Principal;

namespace MoneyManagement.DataAccess
{
    public class AccountRepository : IAccountRepository
    {

        private readonly FinanceContext _financeContext;

        public AccountRepository(FinanceContext financeContext)
        {
            _financeContext = financeContext ?? throw new ArgumentNullException(nameof(financeContext));
        }



        public async Task<List<Account>> LoadAccounts()
        {
            var accounts = await _financeContext.Accounts
                .OrderBy(a => a.Name)
                .ToListAsync();
            
            return accounts;
        }

        public async void SaveAccount(Account account)
        {
            var loadedAccounts = await _financeContext.Accounts.ToListAsync();
            loadedAccounts.Add(account);
            await _financeContext.SaveChangesAsync();
        }
    }
}
