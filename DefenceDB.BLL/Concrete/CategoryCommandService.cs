using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using DefenceDB.EL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.BLL.Concrete;

public class CategoryCommandService : ICategoryCommandService
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cacheService;

    public CategoryCommandService(AppDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    private async Task ClearCategoryCacheAsync()
    {
        await _cacheService.RemoveAsync("categories:all");
        await _cacheService.RemoveAsync("categories:root");
        await _cacheService.RemoveAsync("categories:tree");
        await _cacheService.RemoveAsync("categories:product-counts");
    }

    public async Task AddCategoryAsync(Category category)
    {
        category.CreatedAt = DateTime.UtcNow;
        category.Slug = category.Name.ToSlug();
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        await ClearCategoryCacheAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        category.UpdatedAt = DateTime.UtcNow;
        category.Slug = category.Name.ToSlug();
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        await ClearCategoryCacheAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return;

        // Ürünleri üst kategoriye taşı veya bırak
        var products = await _context.DefenseProducts
            .Where(p => p.CategoryId == id)
            .ToListAsync();
        
        // Alt kategorilerin parent'ını null yap
        var children = await _context.Categories
            .Where(c => c.ParentCategoryId == id)
            .ToListAsync();
        foreach (var child in children)
        {
            child.ParentCategoryId = category.ParentCategoryId;
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        await ClearCategoryCacheAsync();
    }

    // --- Attribute CRUD ---

    public async Task<CategoryAttribute> AddCategoryAttributeAsync(int categoryId, CategoryAttribute attribute)
    {
        attribute.CategoryId = categoryId;
        attribute.CreatedAt = DateTime.UtcNow;
        _context.CategoryAttributes.Add(attribute);
        await _context.SaveChangesAsync();
        await ClearCategoryCacheAsync();
        return attribute;
    }

    public async Task<CategoryAttribute?> UpdateCategoryAttributeAsync(int categoryId, int attributeId, CategoryAttribute updated)
    {
        var existing = await _context.CategoryAttributes.FindAsync(attributeId);
        if (existing == null || existing.CategoryId != categoryId)
            return null;

        var previousName = existing.Name;

        existing.Name = updated.Name;
        existing.DisplayName = updated.DisplayName;
        existing.Type = updated.Type;
        existing.Options = updated.Options;
        existing.IsRequired = updated.IsRequired;
        existing.DisplayOrder = updated.DisplayOrder;
        existing.UpdatedAt = DateTime.UtcNow;

        // Eğer attribute adı değiştiyse, ilgili ürünlerin Specs key'lerini güncelle
        if (!string.Equals(previousName, existing.Name, StringComparison.OrdinalIgnoreCase))
        {
            await RenameSpecOnProductsAsync(categoryId, previousName, existing.Name);
        }

        await _context.SaveChangesAsync();
        await ClearCategoryCacheAsync();
        return existing;
    }

    public async Task<bool> DeleteCategoryAttributeAsync(int categoryId, int attributeId)
    {
        var existing = await _context.CategoryAttributes.FindAsync(attributeId);
        if (existing == null || existing.CategoryId != categoryId)
            return false;

        // 1. İlgili ürünlerden bu spec key'ini sil ve veritabanına kaydet
        await RemoveSpecFromProductsAsync(categoryId, existing.Name);

        // 2. CategoryAttribute kaydını sil
        _context.CategoryAttributes.Remove(existing);
        var result = await _context.SaveChangesAsync() > 0;
        await ClearCategoryCacheAsync();
        return result;
    }

    public async Task SyncProductSpecsWithCategoryAttributesAsync(int categoryId)
    {
        var categoryIds = await GetCategoryDescendantIds(categoryId);
        categoryIds.Add(categoryId);

        // Kategori hiyerarşisinde tanımlı geçerli TÜM attribute isimlerini al
        var validAttributeNames = await _context.CategoryAttributes
            .Where(a => categoryIds.Contains(a.CategoryId))
            .Select(a => a.Name)
            .ToListAsync();

        // Ayrıca üst kategorilerin attribute'larını da al (Inherited)
        var category = await _context.Categories.FindAsync(categoryId);
        var parentId = category?.ParentCategoryId;
        while (parentId.HasValue)
        {
            var parentAttrs = await _context.CategoryAttributes
                .Where(a => a.CategoryId == parentId.Value)
                .Select(a => a.Name)
                .ToListAsync();
            validAttributeNames.AddRange(parentAttrs);

            var parentCategory = await _context.Categories.FindAsync(parentId.Value);
            parentId = parentCategory?.ParentCategoryId;
        }

        var validNamesSet = validAttributeNames.ToHashSet(StringComparer.OrdinalIgnoreCase);

        var products = await _context.DefenseProducts
            .Where(p => categoryIds.Contains(p.CategoryId))
            .ToListAsync();

        bool hasChanges = false;
        foreach (var product in products)
        {
            if (product.Specs == null || !product.Specs.Any()) continue;

            var keysToRemove = product.Specs.Keys
                .Where(k => !validNamesSet.Contains(k))
                .ToList();

            if (keysToRemove.Any())
            {
                foreach (var key in keysToRemove)
                {
                    product.Specs.Remove(key);
                }
                product.Specs = new Dictionary<string, string>(product.Specs);
                product.UpdatedAt = DateTime.UtcNow;
                hasChanges = true;
            }
        }

        if (hasChanges)
        {
            await _context.SaveChangesAsync();
        }
    }

    private async Task<List<int>> GetCategoryDescendantIds(int parentId)
    {
        var children = await _context.Categories
            .Where(c => c.ParentCategoryId == parentId)
            .Select(c => c.Id)
            .ToListAsync();
        var descendants = new List<int>(children);

        foreach (var childId in children)
        {
            descendants.AddRange(await GetCategoryDescendantIds(childId));
        }

        return descendants;
    }

    private async Task RemoveSpecFromProductsAsync(int categoryId, string specName)
    {
        if (string.IsNullOrWhiteSpace(specName)) return;

        var categoryIds = await GetCategoryDescendantIds(categoryId);
        categoryIds.Add(categoryId);

        var products = await _context.DefenseProducts
            .Where(p => categoryIds.Contains(p.CategoryId))
            .ToListAsync();

        foreach (var product in products)
        {
            if (product.Specs != null && product.Specs.Remove(specName))
            {
                product.Specs = new Dictionary<string, string>(product.Specs);
                product.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task RenameSpecOnProductsAsync(int categoryId, string previousName, string nextName)
    {
        if (string.IsNullOrWhiteSpace(previousName) || string.IsNullOrWhiteSpace(nextName)) return;

        var categoryIds = await GetCategoryDescendantIds(categoryId);
        categoryIds.Add(categoryId);

        var products = await _context.DefenseProducts
            .Where(p => categoryIds.Contains(p.CategoryId))
            .ToListAsync();

        foreach (var product in products)
        {
            if (product.Specs != null && product.Specs.TryGetValue(previousName, out var value))
            {
                product.Specs.Remove(previousName);
                product.Specs[nextName] = value;
                product.Specs = new Dictionary<string, string>(product.Specs);
                product.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
    }
}
