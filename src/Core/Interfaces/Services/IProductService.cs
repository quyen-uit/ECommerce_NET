

using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<int> AddProductAsync(Product product, int categoryId, int brandId, int colorId);
        Task<int> UpdateProductAsync(Product product);
        Task<int> DeleteProductAsync(int id);
    }
}
