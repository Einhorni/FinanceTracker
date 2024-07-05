using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.Models
{
    public interface IAccount
    {
        public decimal AddAmount(decimal amount);
        public decimal SubstractAmount(decimal amount);
    }
}
