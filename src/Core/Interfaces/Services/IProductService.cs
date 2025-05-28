

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
        Task<Product> GetProductByIdAsync(long id);
        Task<Product> AddProductAsync(CreateProductDto productDto);
        Task<Product> UpdateProductAsync(long id, CreateProductDto productDto);
        Task<long> DeleteProductAsync(long id);

    }
}
