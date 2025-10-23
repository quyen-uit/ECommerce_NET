using Core.Dtos.ProductSkus;
using Core.Specifications.ProductSkus;

namespace Core.Interfaces.Services
{
    public interface IProductSkuService
    {
        Task<IReadOnlyList<ProductSkuDto>> GetAllProductSkusAsync(ProductSkuSpecParams specParams);
        Task<ProductSkuDto> GetProductSkuByIdAsync(Guid id);
        Task<ProductSkuDto> AddOrUpdateProductSkuAsync(CreateProductSkuDto skuDto);
        Task DeleteProductSkuAsync(Guid id);
    }
}
