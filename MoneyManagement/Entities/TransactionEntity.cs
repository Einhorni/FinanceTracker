using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.Entities
{
    public class TransactionEntity
    {
        [Required]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty ;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryName))]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public AccountEntity Account { get; set; } = default!;

        public Guid? FromAccountId { get; set; } 
        public Guid? ToAccountId { get; set; }
    }
}
