using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.Models
{
    public class Category
    {
        public string Name { get; set; } = string.Empty;
        [Required]
        public bool Expense { get; set; }
    }
}
