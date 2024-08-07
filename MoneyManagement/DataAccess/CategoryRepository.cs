using Microsoft.EntityFrameworkCore;
using MoneyManagement.DbContexts;
using MoneyManagement.Entities;
using MoneyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagement.DataAccess
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FinanceContext _financeContext;

        public CategoryRepository(FinanceContext financeContext)
        {
            _financeContext = financeContext ?? throw new ArgumentNullException(nameof(financeContext));
        }

        public async Task<List<Category>> GetCategories()
        {
            var categoryEntities =
                await _financeContext.Categories
                //.Where(c => c.Expense == true)
                .ToListAsync();
            var categories = categoryEntities.Select(c => new Category { Name = c.Name, Expense = c.Expense }).ToList();
            return categories;
        }
    }
}
