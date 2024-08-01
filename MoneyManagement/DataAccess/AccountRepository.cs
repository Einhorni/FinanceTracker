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
            _financeContext = financeContext;
        }



        //TODO: transaction müssen geladen werden, um die aktuelle balance herauszufinden -> eine schickt höher und dann als parameter mitgegeben?
        public async Task<List<Account>> LoadAccounts()
        {
            var accounts = await _financeContext.Accounts
                .OrderBy(a => a.Name)
                .ToListAsync();

            

            return accounts;
        }

        //public void SaveAccounts(List<AccountDTO> accounts);
    }
}
