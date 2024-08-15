using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoneyManagement.BusinessModels;
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
            //load nimmt nichts oder id entgegen und gibt domain objekt aus
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
                    a.AccountEntityToAccount())
                .ToList();

            return accounts;
        }

        // CodeReview: Function schlägt fehl, wenn id unbekannt.
        // bei nicht vorhanden sein, null zurückgeben und in der Schicht oben drüber adequat als User-Fehler behandeln. "Datensatz nicht vorhanden", oder ähnlich
        public async Task<Account> LoadAccount(Guid id)
        {
            var accountEntity = await _financeContext.Accounts
                .Include(a => a.Transactions)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            //bei allen Ladeoperationen: account oder null zurückgeben (auch im service), wenn etwas nicht geladen werden kann -> Nachricht in UI, zusätzlich try catch
            if (accountEntity == null)
            { 
                throw new ArgumentNullException(nameof(accountEntity));
            }

            var account = accountEntity?.AccountEntityToAccount(); 

            return account;
        }

        // CodeReview: prüfen ob input null ist und wenn ja ArgumentNullException werfen, try catch in UI
        // Update inmplementieren
        public async Task SaveAccount(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            var accountEntity = account.AccountToAccountEntity();

            await _financeContext.Accounts.AddAsync(accountEntity);
            await _financeContext.SaveChangesAsync();
        }
    }
}
