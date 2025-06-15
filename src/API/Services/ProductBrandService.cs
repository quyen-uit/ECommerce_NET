using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.ProductBrands;
using Mapster;

namespace API.Services
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly IGenericRepository<ProductBrand> _brandRepository;

        public ProductBrandService(IGenericRepository<ProductBrand> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<ProductBrandDto> AddProductBrandAsync(CreateProductBrandDto brandDto)
        {
            var brand = brandDto.Adapt<ProductBrand>();
            _brandRepository.Add(brand);
            await _brandRepository.Complete();
            return brand.Adapt<ProductBrandDto>();
        }

        public async Task<IReadOnlyList<ProductBrandDto>> AddRangeProductBrandAsync(
            IReadOnlyList<CreateProductBrandDto> brandDtos
        )
        {
            var brands = brandDtos.Adapt<IReadOnlyList<ProductBrand>>();
            _brandRepository.AddRange(brands);
            await _brandRepository.Complete();
            return brands.Adapt<IReadOnlyList<ProductBrandDto>>();
        }

        public async Task<int> CountAllAsync(ProductBrandSpecParams specParams)
        {
            var spec = new ProductBrandWithParamsSpec(specParams);
            return await _brandRepository.CountAsync(spec);
        }

        public async Task<bool> DeleteProductBrandAsync(long id)
        {
            var existing = await _brandRepository.GetByIdAsync(id);
            if (existing == null)
                return false;

            _brandRepository.Delete(id);
            await _brandRepository.Complete();
            return true;
        }

        public async Task<IReadOnlyList<ProductBrandDto>> GetAllProductBrandsAsync(
            ProductBrandSpecParams specParams
        )
        {
            var spec = new ProductBrandWithParamsSpec(specParams);
            var brands = await _brandRepository.GetAllWithSpecAsync(spec);
            return brands.Adapt<IReadOnlyList<ProductBrandDto>>();
        }

        public async Task<ProductBrandDto> GetProductBrandByIdAsync(long id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            return brand?.Adapt<ProductBrandDto>();
        }

        public async Task<ProductBrandDto> UpdateProductBrandAsync(UpdateProductBrandDto brandDto)
        {
            var existing = await _brandRepository.GetByIdAsync(brandDto.Id);
            if (existing == null)
                return null;

            existing = brandDto.Adapt<ProductBrand>();
            _brandRepository.Update(existing);
            await _brandRepository.Complete();
            return existing.Adapt<ProductBrandDto>();
        }
    }
}
