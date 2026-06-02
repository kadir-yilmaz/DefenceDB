using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.BLL.Concrete;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
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

    public async Task AddProductAsync(DefenseProduct product)
    {
        product.CreatedAt = DateTime.UtcNow;
        _context.DefenseProducts.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(DefenseProduct product)
    {
        var existing = await _context.DefenseProducts.FindAsync(product.Id);
        if (existing != null)
        {
            // For TPT, we might want to attach and set state to modified.
            _context.Entry(existing).CurrentValues.SetValues(product);
            existing.UpdatedAt = DateTime.UtcNow;

            // Yeni eklenen resimleri (Id == 0 olanlar) kaydet
            if (product.Images != null)
            {
                bool hasNewMainImage = product.Images.Any(img => img.Id == 0 && img.IsMainImage);
                if (hasNewMainImage)
                {
                    var existingImages = await _context.ProductImages
                        .Where(i => i.ProductId == product.Id)
                        .ToListAsync();
                    foreach (var img in existingImages)
                    {
                        img.IsMainImage = false;
                    }
                }

                foreach (var img in product.Images)
                {
                    if (img.Id == 0)
                    {
                        _context.ProductImages.Add(img);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.DefenseProducts.FindAsync(id);
        if (product != null)
        {
            _context.DefenseProducts.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateProductRelationshipsAsync(int sourceProductId, List<int> targetProductIds)
    {
        // İki yönlü bağlantıları tamamen temizle ki tutarsızlık kalmasın
        var existingRelations = await _context.ProductRelationships
            .Where(r => r.SourceProductId == sourceProductId || r.TargetProductId == sourceProductId)
            .ToListAsync();

        _context.ProductRelationships.RemoveRange(existingRelations);
        
        // Formdan gelen (seçili) bağlantıları yeniden oluştur
        foreach(var targetId in targetProductIds)
        {
            _context.ProductRelationships.Add(new ProductRelationship 
            {
                SourceProductId = sourceProductId,
                TargetProductId = targetId,
                RelationType = "Bağlantılı Donanım/Mühimmat",
                CreatedAt = DateTime.UtcNow
            });
        }
        await _context.SaveChangesAsync();
    }

    public async Task<ProductImage?> GetProductImageByIdAsync(int imageId)
    {
        return await _context.ProductImages.FindAsync(imageId);
    }

    public async Task DeleteProductImageAsync(ProductImage image)
    {
        _context.ProductImages.Remove(image);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductImagesAsync(IEnumerable<int> imageIds)
    {
        var imagesToDelete = await _context.ProductImages
            .Where(i => imageIds.Contains(i.Id))
            .ToListAsync();

        if (imagesToDelete.Any())
        {
            _context.ProductImages.RemoveRange(imagesToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SetMainImageAsync(int productId, int mainImageId)
    {
        var images = await _context.ProductImages
            .Where(i => i.ProductId == productId)
            .ToListAsync();

        foreach (var img in images)
        {
            img.IsMainImage = (img.Id == mainImageId);
        }

        await _context.SaveChangesAsync();
    }
}
