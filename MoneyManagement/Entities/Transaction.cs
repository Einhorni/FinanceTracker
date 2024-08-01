using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.Entities
{
    public class Transaction
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty ;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey("Category")]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string AccountId { get; set; } = string.Empty;

        public string? FromAccountId { get; set; } 
        public string? ToAccountId { get; set; } 
    }
}
