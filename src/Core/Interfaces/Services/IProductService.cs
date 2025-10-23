

using Core.Common;
using Core.Dtos.Products;
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
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<ProductDto> AddOrUpdateProductAsync(CreateProductDto productDto);
        Task DeleteProductAsync(Guid id);

    }
}
