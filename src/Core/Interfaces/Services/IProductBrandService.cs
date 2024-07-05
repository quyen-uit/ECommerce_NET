using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IProductBrandService
    {
        Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync();
        Task<ProductBrand> GetProductBrandByIdAsync(int id);
        Task<int> AddProductBrandAsync(ProductBrand productBrand);
        Task<int> AddRangeProductBrandAsync(IReadOnlyList<ProductBrand> productBrand);
        Task<int> UpdateProductBrandAsync(ProductBrand productBrand);
        Task<int> DeleteProductBrandAsync(int id);
    }

}
