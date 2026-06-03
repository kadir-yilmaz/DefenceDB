using DefenceDB.EL.Models;

namespace DefenceDB.BLL.Abstract;

/// <summary>
/// Ürün arama ve listeleme soyutlaması.
/// Elasticsearch aktifse ElasticsearchService, değilse SqlFallbackSearchService kullanılır.
/// </summary>
public interface ISearchService
{
    /// <summary>Tüm ürünleri listele (denormalize, flat)</summary>
    Task<List<ProductDocument>> GetAllProductsAsync();

    /// <summary>Kategori bazında ürünleri getir</summary>
    Task<List<ProductDocument>> GetProductsByCategoryAsync(int categoryId);

    /// <summary>Tam metin arama</summary>
    Task<List<ProductDocument>> SearchAsync(string query, int maxResults = 20);

    /// <summary>Elasticsearch'e tüm ürünleri yeniden indeksle</summary>
    Task ReindexAllAsync();

    /// <summary>Tek bir ürünü indeksle/güncelle</summary>
    Task IndexProductAsync(ProductDocument product);

    /// <summary>Tek bir ürünü indeksten sil</summary>
    Task RemoveProductAsync(int productId);
}
