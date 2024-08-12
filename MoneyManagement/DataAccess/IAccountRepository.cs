using MoneyManagement.Entities;
using MoneyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.DataAccess
{
    public interface IAccountRepository
    {
        Task<Account> LoadAccount(Guid id);
        public Task<List<Account>> LoadAccounts();
        public Task SaveAccount(Account account);
    }
}
