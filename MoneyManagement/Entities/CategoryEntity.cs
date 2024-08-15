using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.Entities
{
    public class CategoryEntity
    {
        [Required]
        [Key]
        public string Name { get; set; } = string.Empty;
        [Required]
        public bool Expense {  get; set; }
    }
}
