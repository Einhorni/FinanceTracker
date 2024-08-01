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
        public Task<List<Account>> LoadAccounts();
        //public void SaveAccounts(List<Account> accounts);
    }
}
