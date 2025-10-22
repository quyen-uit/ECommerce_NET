using Core.Dtos.ProductSkus;
using Core.Specifications.ProductSkus;

namespace Core.Interfaces.Services
{
    public interface IProductSkuService
    {
        Task<IReadOnlyList<ProductSkuDto>> GetAllProductSkusAsync(ProductSkuSpecParams specParams);
        Task<ProductSkuDto> GetProductSkuByIdAsync(long id);
        Task<ProductSkuDto> AddOrUpdateProductSkuAsync(CreateProductSkuDto skuDto);
        Task DeleteProductSkuAsync(long id);
    }
}
