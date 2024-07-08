

using Core.Dtos;
using Core.Dtos.CreateDto;
using Core.Entities;
using Core.Specifications.Products;

namespace Core.Interfaces.Services
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync(ProductSpecParams productSpecParams);
        Task<int> CountAllProductsAsync(ProductSpecParams productSpecParams);
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(CreateProductDto productDto);
        Task<int> UpdateProductAsync(int id, CreateProductDto product);
        Task<int> DeleteProductAsync(int id);
    }
}
