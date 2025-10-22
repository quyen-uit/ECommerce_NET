using Core.Common;
using Core.Constants;
using Core.Dtos.ProductBrands;
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

        public async Task<ProductBrandDto> AddOrUpdateProductBrandAsync(CreateProductBrandDto dto)
        {
            var entity = await _brandRepository.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                entity = dto.Adapt<ProductBrand>();
                _brandRepository.Add(entity);
            }
            else
            {
                dto.Adapt(entity);
                _brandRepository.Update(entity);
            }
            await _brandRepository.Complete();
            return entity.Adapt<ProductBrandDto>();
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
            var spec = new ProductBrandSpecification(specParams);
            return await _brandRepository.CountAsync(spec);
        }

        public async Task DeleteProductBrandAsync(long id)
        {
            var existing = await _brandRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundBrand);

            _brandRepository.Delete(existing);
            await _brandRepository.Complete();
        }

        public async Task<Pagination<ProductBrandDto>> GetAllProductBrandsAsync(
            ProductBrandSpecParams specParams
        )
        {
            var spec = new ProductBrandSpecification(specParams);
            var brands = await _brandRepository.GetAllWithSpecAsync(spec);
            var count = await _brandRepository.CountAsync(spec);
            return new Pagination<ProductBrandDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: brands.Adapt<IReadOnlyList<ProductBrandDto>>()
                );
        }

        public async Task<ProductBrandDto> GetProductBrandByIdAsync(long id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
                throw new NotFoundException(CommonMessage.NotFoundBrand);
            return brand.Adapt<ProductBrandDto>();
        }

        public async Task DeleteBrandsAsync(List<long> ids)
        {
            await _brandRepository.DeleteRangeById(ids);
            await _brandRepository.Complete();
        }
    }
}
