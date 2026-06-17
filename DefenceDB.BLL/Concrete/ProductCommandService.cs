using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace DefenceDB.BLL.Concrete;

public class ProductCommandService : IProductCommandService
{
    private readonly AppDbContext _context;

    public ProductCommandService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddProductAsync(DefenseProduct product)
    {
        product.CreatedAt = DateTime.UtcNow;
        _context.DefenseProducts.Add(product);
        await _context.SaveChangesAsync();
        await UpsertReadModelAsync(product.Id);
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
            await UpsertReadModelAsync(product.Id);
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.DefenseProducts.FindAsync(id);
        if (product != null)
        {
            _context.DefenseProducts.Remove(product);
            var readModel = await _context.ProductReadModels.FindAsync(id);
            if (readModel != null)
                _context.ProductReadModels.Remove(readModel);

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
        var productId = image.ProductId;
        _context.ProductImages.Remove(image);
        await _context.SaveChangesAsync();
        await UpsertReadModelAsync(productId);
    }

    public async Task DeleteProductImagesAsync(IEnumerable<int> imageIds)
    {
        var imagesToDelete = await _context.ProductImages
            .Where(i => imageIds.Contains(i.Id))
            .ToListAsync();

        if (imagesToDelete.Any())
        {
            var productIds = imagesToDelete.Select(i => i.ProductId).Distinct().ToList();
            _context.ProductImages.RemoveRange(imagesToDelete);
            await _context.SaveChangesAsync();

            foreach (var productId in productIds)
            {
                await UpsertReadModelAsync(productId);
            }
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
        await UpsertReadModelAsync(productId);
    }

    private async Task UpsertReadModelAsync(int productId)
    {
        var product = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
            return;

        var readModel = await _context.ProductReadModels.FindAsync(productId);
        if (readModel == null)
        {
            readModel = new ProductReadModel { Id = product.Id };
            _context.ProductReadModels.Add(readModel);
        }

        readModel.Name = product.Name;
        readModel.Slug = product.Slug;
        readModel.NatoReportingName = product.NatoReportingName;
        readModel.Description = product.Description;
        readModel.Country = product.Country;
        readModel.Manufacturer = product.Manufacturer;
        readModel.YearIntroduced = product.YearIntroduced;
        readModel.ThumbnailUrl = product.ThumbnailUrl;
        readModel.Status = product.Status;
        readModel.IsActive = product.IsActive;
        readModel.IsShowcase = product.IsShowcase;
        readModel.VideoUrl = product.VideoUrl;
        readModel.CategoryId = product.CategoryId;
        readModel.CategoryName = product.Category?.Name ?? "";
        readModel.CategorySlug = product.Category?.Slug ?? "";
        readModel.ProductType = product.GetType().Name;
        readModel.MainImageUrl = product.Images?.FirstOrDefault(i => i.IsMainImage)?.ImagePath
                                 ?? product.Images?.FirstOrDefault()?.ImagePath;
        readModel.CreatedAt = product.CreatedAt;
        readModel.UpdatedAt = product.UpdatedAt;
        readModel.SpecificPropertiesJson = JsonSerializer.Serialize(GetSpecificProperties(product));

        await _context.SaveChangesAsync();
    }

    private static Dictionary<string, object?> GetSpecificProperties(DefenseProduct product)
    {
        var baseProperties = typeof(DefenseProduct)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToHashSet();

        return product.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => !baseProperties.Contains(p.Name))
            .ToDictionary(p => p.Name, p => p.GetValue(product));
    }
}
