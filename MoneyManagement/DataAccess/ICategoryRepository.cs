using MoneyManagement.Entities;

namespace MoneyManagement.DataAccess
{
    public interface ICategoryRepository
    {
        Task<List<Models.Category>> GetCategories();
    }
}