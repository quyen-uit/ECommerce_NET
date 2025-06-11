using Core.Dtos;
using Core.Entities;
using Core.Specifications.ProductBrands;

namespace Core.Interfaces.Services
{
    public interface IProductBrandService
    {
        Task<IReadOnlyList<ProductBrandDto>> GetAllProductBrandsAsync(
            ProductBrandSpecParams specParams
        );
        Task<int> CountAllAsync(ProductBrandSpecParams specParams);
        Task<ProductBrandDto> GetProductBrandByIdAsync(long id);
        Task<ProductBrandDto> AddProductBrandAsync(CreateProductBrandDto brandDto);
        Task<IReadOnlyList<ProductBrandDto>> AddRangeProductBrandAsync(
            IReadOnlyList<CreateProductBrandDto> brandDtos
        );
        Task<ProductBrandDto> UpdateProductBrandAsync(UpdateProductBrandDto brandDto);
        Task<bool> DeleteProductBrandAsync(long id);
    }
}
