using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoneyManagement.DbContexts;
using MoneyManagement.Entities;
using MoneyManagement.Models;
using System.Linq;
using System.Security.Principal;

namespace MoneyManagement.DataAccess
{
    public class AccountRepository : IAccountRepository
    {

        //repo pattern: nimmt business objekte entgegen oder gibt sie aus.
            //load nimmt nichts oder id entgegen und gibt domain objet aus
            //save nimmt domainobjekt an und speichert es über dbcontext
            //es hat nichts mit Entities zu tun
        //repo gehört zur Datenebene

        private readonly FinanceContext _financeContext;

        public AccountRepository(FinanceContext financeContext)
        {
            _financeContext = financeContext ?? throw new ArgumentNullException(nameof(financeContext));
        }



        public async Task<List<Account>> LoadAccounts()
        {
            //mit dem Include wird die Liste der Transaktionen mit in das Objekt geladen
            var accountEntities = await _financeContext.Accounts
                .Include(a => a.Transactions)
                .OrderBy(a => a.Name)
                .ToListAsync();

            //Maps accountEntities to Account, while creating the Balance from its transactions
            var accounts = accountEntities
                .Select(a =>
                    a.AccountEntityToAccount(
                        a.Transactions
                        .Select(t => t.Amount).Sum()))
                .ToList();

            return accounts;
        }

        public async void SaveAccount(Account account)
        {
            var accountEntity = account.AccountToAccountEntity();

            await _financeContext.Accounts.AddAsync(accountEntity);
            await _financeContext.SaveChangesAsync();
        }
    }
}
