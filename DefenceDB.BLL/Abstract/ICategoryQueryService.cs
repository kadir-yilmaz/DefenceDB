using DefenceDB.EL.Models;

namespace DefenceDB.BLL.Abstract;

public interface ICategoryQueryService
{
    Task<List<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category?> GetCategoryBySlugAsync(string slug);
    Task<List<Category>> GetRootCategoriesAsync();
    Task<List<Category>> GetCategoriesWithChildrenAsync();
    Task<Category?> GetCategoryWithSubCategoriesAsync(int id);
    Task<Dictionary<int, int>> GetCategoryProductCountsAsync();
}
