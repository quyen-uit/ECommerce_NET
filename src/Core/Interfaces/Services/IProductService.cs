

using Core.Common;
using Core.Dtos;
using Core.Entities;
using Core.Specifications.Products;

namespace Core.Interfaces.Services
{
    public interface IProductService
    {
        #region Admin
        Task<Pagination<ProductDto>> GetAllProductFilterByNameAsync(ProductFilterByNameSpecParams productSpecParams);
        #endregion
        Task<Pagination<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpecParams);
        // Task<int> CountAllProductsAsync(ProductSpecParams productSpecParams);
        Task<ProductDto> GetProductByIdAsync(long id);
        Task<ProductDto> AddProductAsync(CreateProductDto productDto);
        Task<ProductDto> UpdateProductAsync(UpdateProductDto productDto);
        Task DeleteProductAsync(long id);

    }
}
