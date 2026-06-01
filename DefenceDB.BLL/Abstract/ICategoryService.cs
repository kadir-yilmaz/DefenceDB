using DefenceDB.EL.Models;

namespace DefenceDB.BLL.Abstract;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category?> GetCategoryBySlugAsync(string slug);
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
    Task<List<Category>> GetRootCategoriesAsync();
    Task<List<Category>> GetCategoriesWithChildrenAsync();
}
