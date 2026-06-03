using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.BLL.Concrete;

public class ProductCommandManager : IProductCommandService
{
    private readonly AppDbContext _context;

    public ProductCommandManager(AppDbContext context)
    {
        _context = context;
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
            _context.Entry(existing).CurrentValues.SetValues(product);
            existing.UpdatedAt = DateTime.UtcNow;

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
        var existingRelations = await _context.ProductRelationships
            .Where(r => r.SourceProductId == sourceProductId || r.TargetProductId == sourceProductId)
            .ToListAsync();

        _context.ProductRelationships.RemoveRange(existingRelations);
        
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
