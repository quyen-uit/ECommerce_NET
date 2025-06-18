using Core.Dtos;
using Core.Specifications.ProductSkus;

namespace Core.Interfaces.Services
{
    public interface IProductSkuService
    {
        Task<IReadOnlyList<ProductSkuDto>> GetAllProductSkusAsync(ProductSkuSpecParams specParams);
        Task<ProductSkuDto> GetProductSkuByIdAsync(long id);
        Task<ProductSkuDto> AddProductSkuAsync(CreateProductSkuDto skuDto);
        Task<ProductSkuDto> UpdateProductSkuAsync(UpdateProductSkuDto skuDto);
        Task DeleteProductSkuAsync(long id);
    }
}
