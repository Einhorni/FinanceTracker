using Microsoft.EntityFrameworkCore;
using MoneyManagement.DbContexts;
using MoneyManagement.Entities;
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

        public async Task<List<CategoryEntity>> GetCategories()
        {
            var categories = await _financeContext.Categories.ToListAsync();
            return categories;
        }
    }
}
