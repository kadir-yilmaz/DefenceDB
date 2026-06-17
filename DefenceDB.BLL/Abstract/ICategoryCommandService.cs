using DefenceDB.EL.Models;

namespace DefenceDB.BLL.Abstract;

public interface ICategoryCommandService
{
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
}
