using DefenceDB.EL.Models;

namespace DefenceDB.BLL.Abstract;

public interface IProductService
{
    Task<List<DefenseProduct>> GetAllProductsAsync();
    IQueryable<DefenseProduct> GetProductsQueryable();
    Task<List<DefenseProduct>> GetProductsByCategoryAsync(int categoryId);
    Task<List<DefenseProduct>> GetProductsByCategorySlugAsync(string categorySlug);
    Task<DefenseProduct?> GetProductByIdAsync(int id);
    Task<DefenseProduct?> GetProductBySlugAsync(string slug);
    
    Task<List<DefenseProduct>> GetRecentProductsAsync(int count = 6);
    Task<List<DefenseProduct>> GetShowcaseProductsAsync();
    Task<List<DefenseProduct>> SearchProductsAsync(string query);

    Task AddProductAsync(DefenseProduct product);
    Task UpdateProductAsync(DefenseProduct product);
    Task DeleteProductAsync(int id);

    Task UpdateProductRelationshipsAsync(int sourceProductId, List<int> targetProductIds);
    Task<ProductImage?> GetProductImageByIdAsync(int imageId);
    Task DeleteProductImageAsync(ProductImage image);
    Task DeleteProductImagesAsync(IEnumerable<int> imageIds);
    Task SetMainImageAsync(int productId, int mainImageId);
}
