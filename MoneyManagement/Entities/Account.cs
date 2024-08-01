using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.Entities
{
    public class Account
    {
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public DateTime DateOfCreation { get; set; }

        [Required]
        public string KindOfAccount { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Currency { get; set; } = string.Empty;

        public decimal Overdraft { get; set; }
        public double InterestRate { get; set; }

        //in months
        public int InterestInterval { get; set; }
        public int InvestmentDuration { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
