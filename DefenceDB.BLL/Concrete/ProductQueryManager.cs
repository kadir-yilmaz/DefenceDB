using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.BLL.Concrete;

public class ProductQueryManager : IProductQueryService
{
    private readonly AppDbContext _context;

    public ProductQueryManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DefenseProduct>> GetAllProductsAsync()
    {
        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public IQueryable<DefenseProduct> GetProductsQueryable()
    {
        return _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .AsQueryable();
    }

    public async Task<List<DefenseProduct>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<List<DefenseProduct>> GetProductsByCategorySlugAsync(string categorySlug)
    {
        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.Category.Slug == categorySlug)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<DefenseProduct?> GetProductByIdAsync(int id)
    {
        return await _context.DefenseProducts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.SourceRelationships)
                .ThenInclude(r => r.TargetProduct)
                    .ThenInclude(tp => tp.Images)
            .Include(p => p.SourceRelationships)
                .ThenInclude(r => r.TargetProduct)
                    .ThenInclude(tp => tp.Category)
            .Include(p => p.TargetRelationships)
                .ThenInclude(r => r.SourceProduct)
                    .ThenInclude(sp => sp.Images)
            .Include(p => p.TargetRelationships)
                .ThenInclude(r => r.SourceProduct)
                    .ThenInclude(sp => sp.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<DefenseProduct?> GetProductBySlugAsync(string slug)
    {
        return await _context.DefenseProducts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.SourceRelationships)
                .ThenInclude(r => r.TargetProduct)
                    .ThenInclude(tp => tp.Images)
            .Include(p => p.SourceRelationships)
                .ThenInclude(r => r.TargetProduct)
                    .ThenInclude(tp => tp.Category)
            .Include(p => p.TargetRelationships)
                .ThenInclude(r => r.SourceProduct)
                    .ThenInclude(sp => sp.Images)
            .Include(p => p.TargetRelationships)
                .ThenInclude(r => r.SourceProduct)
                    .ThenInclude(sp => sp.Category)
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<List<DefenseProduct>> GetRecentProductsAsync(int count = 6)
    {
        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<DefenseProduct>> GetShowcaseProductsAsync()
    {
        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.IsActive && p.IsShowcase)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<DefenseProduct>> SearchProductsAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<DefenseProduct>();

        query = query.ToLower();

        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => 
                p.Name.ToLower().Contains(query) || 
                (p.Description != null && p.Description.ToLower().Contains(query)) ||
                (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(query)) ||
                (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(query))
            )
            .OrderBy(p => p.Name)
            .Take(20)
            .ToListAsync();
    }

    public async Task<ProductImage?> GetProductImageByIdAsync(int imageId)
    {
        return await _context.ProductImages.FindAsync(imageId);
    }
}
