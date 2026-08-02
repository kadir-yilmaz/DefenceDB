using DefenceDB.EL.Models;

namespace DefenceDB.BLL.Abstract;

public interface IProductQueryService
{
    Task<List<DefenseProduct>> GetAllProductsAsync();
    Task<List<DefenseProduct>> GetProductsByCategoryAsync(int categoryId);
    Task<List<DefenseProduct>> GetProductsByCategorySlugAsync(string categorySlug);
    Task<DefenseProduct?> GetProductByIdAsync(int id);
    Task<DefenseProduct?> GetProductBySlugAsync(string slug);
    Task<List<DefenseProduct>> GetRecentProductsAsync(int count = 6);
    Task<List<DefenseProduct>> GetShowcaseProductsAsync();
    Task<List<DefenseProduct>> SearchProductsAsync(string query);
    Task<List<DefenseProduct>> SearchSuggestionsAsync(string term, int maxResults = 8);
    Task<ProductImage?> GetProductImageByIdAsync(int imageId);
    Task<(List<DefenseProduct> Products, int TotalItems)> GetFilteredProductsAsync(ProductFilterQueryModel queryModel);
}
