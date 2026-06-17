using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.BLL.Concrete;

public class CategoryQueryService : ICategoryQueryService
{
    private readonly AppDbContext _context;

    public CategoryQueryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .Include(c => c.SubCategories)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .Include(c => c.SubCategories)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category?> GetCategoryBySlugAsync(string slug)
    {
        return await _context.Categories
            .AsNoTracking()
            .Include(c => c.SubCategories)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Slug == slug);
    }

    public async Task<List<Category>> GetRootCategoriesAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == null)
            .Include(c => c.Products)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();
    }

    public async Task<List<Category>> GetCategoriesWithChildrenAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == null)
            .Include(c => c.SubCategories.OrderBy(sc => sc.SortOrder))
                .ThenInclude(sc => sc.Products)
            .Include(c => c.Products)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryWithSubCategoriesAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
