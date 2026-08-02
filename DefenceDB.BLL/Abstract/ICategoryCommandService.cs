using DefenceDB.EL.Models;

namespace DefenceDB.BLL.Abstract;

public interface ICategoryCommandService
{
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);

    // Attribute CRUD
    Task<CategoryAttribute> AddCategoryAttributeAsync(int categoryId, CategoryAttribute attribute);
    Task<CategoryAttribute?> UpdateCategoryAttributeAsync(int categoryId, int attributeId, CategoryAttribute attribute);
    Task<bool> DeleteCategoryAttributeAsync(int categoryId, int attributeId);
    Task SyncProductSpecsWithCategoryAttributesAsync(int categoryId);
}
