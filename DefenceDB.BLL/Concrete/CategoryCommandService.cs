using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.BLL.Concrete;

public class CategoryCommandService : ICategoryCommandService
{
    private readonly AppDbContext _context;

    public CategoryCommandService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddCategoryAsync(Category category)
    {
        category.CreatedAt = DateTime.UtcNow;
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        category.UpdatedAt = DateTime.UtcNow;
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
