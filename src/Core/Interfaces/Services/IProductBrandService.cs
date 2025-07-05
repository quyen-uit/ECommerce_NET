using Core.Common;
using Core.Dtos;
using Core.Dtos.ProductBrands;
using Core.Entities;
using Core.Specifications.ProductBrands;

namespace Core.Interfaces.Services
{
    public interface IProductBrandService
    {
        Task<Pagination<ProductBrandDto>> GetAllProductBrandsAsync(
            ProductBrandSpecParams specParams
        );
        Task<int> CountAllAsync(ProductBrandSpecParams specParams);
        Task<ProductBrandDto> GetProductBrandByIdAsync(long id);
        Task<ProductBrandDto> AddOrUpdateProductBrandAsync(CreateProductBrandDto brandDto);
        Task<IReadOnlyList<ProductBrandDto>> AddRangeProductBrandAsync(
            IReadOnlyList<CreateProductBrandDto> brandDtos
        );
        Task DeleteProductBrandAsync(long id);
        Task DeleteBrandsAsync(List<long> ids);
    }
}
