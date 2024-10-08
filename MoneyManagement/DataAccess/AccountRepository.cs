﻿using CSharpFunctionalExtensions;
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
            //wenn ich das entitiyobjekt nicht rausgebe, sondern k. a. ein Domänenobjekt
            var accountEntities = await _financeContext.Accounts
                .AsNoTracking()
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


        public async Task<Maybe<Account>> LoadAccount(Guid id) 
        {
            var accountEntity = await _financeContext.Accounts
                .AsNoTracking()
                .Include(a => a.Transactions)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            var account = accountEntity?.AccountEntityToAccount(); 

            return account.AsMaybe();
        }


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
