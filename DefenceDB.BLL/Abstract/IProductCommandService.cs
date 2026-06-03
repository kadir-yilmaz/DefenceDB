using DefenceDB.EL.Models;

namespace DefenceDB.BLL.Abstract;

public interface IProductCommandService
{
    Task AddProductAsync(DefenseProduct product);
    Task UpdateProductAsync(DefenseProduct product);
    Task DeleteProductAsync(int id);

    Task UpdateProductRelationshipsAsync(int sourceProductId, List<int> targetProductIds);
    Task DeleteProductImageAsync(ProductImage image);
    Task DeleteProductImagesAsync(IEnumerable<int> imageIds);
    Task SetMainImageAsync(int productId, int mainImageId);
}
