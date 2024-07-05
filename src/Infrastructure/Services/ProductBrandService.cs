using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Products;

namespace Infrastructure.Services
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;

        public ProductBrandService(IGenericRepository<ProductBrand> productBrandRepository)
        {
            _productBrandRepository = productBrandRepository;
        }

        public async Task<int> AddProductBrandAsync(ProductBrand productBrand)
        {
            _productBrandRepository.Add(productBrand);
            return await _productBrandRepository.Complete();
        }

        public async Task<int> DeleteProductBrandAsync(int id)
        {
            _productBrandRepository.Delete(id);
            return await _productBrandRepository.Complete();
        }

        public async Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync()
        {
            return await _productBrandRepository.GetAllAsync();
        }

        public async Task<int> UpdateProductBrandAsync(ProductBrand productBrand)
        {
            _productBrandRepository.Update(productBrand);
            return await _productBrandRepository.Complete();
        }

        public async Task<int> AddRangeProductBrandAsync(IReadOnlyList<ProductBrand> productBrands)
        {
            _productBrandRepository.AddRange(productBrands);
            return await _productBrandRepository.Complete();
        }

        public async Task<ProductBrand> GetProductBrandByIdAsync(int id)
        {
            return await _productBrandRepository.GetByIdAsync(id);
        }
    }
}
