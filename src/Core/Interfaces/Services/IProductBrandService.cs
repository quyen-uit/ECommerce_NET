using Core.Dtos;
using Core.Dtos.CreateDto;
using Core.Entities;
using Core.Specifications.Categories;

namespace Core.Interfaces.Services
{
    public interface IProductBrandService
    {
        Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync(ProductBrandSpecParams specParams);
        Task<ProductBrand> GetProductBrandByIdAsync(int id);
        Task<ProductBrand> AddProductBrandAsync(CreateProductBrandDto productBrandDto);
        Task<IReadOnlyList<ProductBrand>> AddRangeProductBrandAsync(IReadOnlyList<CreateProductBrandDto> productBrandDto);
        Task<ProductBrand> UpdateProductBrandAsync(int id, CreateProductBrandDto productBrandDto);
        Task<int> DeleteProductBrandAsync(int id);
        Task<int> CountAllAsync(ProductBrandSpecParams specParams);
    }

}
