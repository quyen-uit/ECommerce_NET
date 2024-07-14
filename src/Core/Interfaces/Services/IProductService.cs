

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
        Task<Product> UpdateProductAsync(int id, CreateProductDto productDto);
        Task<int> DeleteProductAsync(int id);

        //product color
        Task<ProductColor> AddProductColorAsync(CreateProductColorDto productColorDto);
        Task<ProductColor> UpdateProductColorAsync(int id, CreateProductColorDto productColorDto);
        Task<ProductColor> GetProductColorsByIdAsync(int productId);
        Task<int> DeleteProductColorAsync(int id);

    }
}
