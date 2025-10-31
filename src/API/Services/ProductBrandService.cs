using Core.Common;
using Core.Constants;
using Core.Dtos.ProductBrands;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Specifications.ProductBrands;
using Core.Interfaces.Reposiories;
using Mapster;

namespace API.Services
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly IRepository<ProductBrand> _brandRepository;

        public ProductBrandService(IRepository<ProductBrand> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<ProductBrandDto> AddOrUpdateProductBrandAsync(CreateProductBrandDto dto)
        {
            ProductBrand? entity = null;
            if (dto.Id.HasValue && dto.Id != Guid.Empty)
            {
                entity = await _brandRepository.GetByIdAsync(dto.Id.Value);
            }
            if (entity == null)
            {
                entity = dto.Adapt<ProductBrand>();
                await _brandRepository.AddAsync(entity);
            }
            else
            {
                dto.Adapt(entity);
                await _brandRepository.UpdateAsync(entity);
            }
            return entity.Adapt<ProductBrandDto>();
        }

        public async Task<IReadOnlyList<ProductBrandDto>> AddRangeProductBrandAsync(
            IReadOnlyList<CreateProductBrandDto> brandDtos
        )
        {
            var brands = brandDtos.Adapt<IReadOnlyList<ProductBrand>>();
            await _brandRepository.AddRangeAsync(brands);
            return brands.Adapt<IReadOnlyList<ProductBrandDto>>();
        }

        public async Task<int> CountAllAsync(ProductBrandSpecParams specParams)
        {
            var spec = new ProductBrandSpecification(specParams);
            return await _brandRepository.CountAsync(spec);
        }

        public async Task DeleteProductBrandAsync(Guid id)
        {
            var existing = await _brandRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundBrand);

            await _brandRepository.DeleteAsync(existing);
        }

        public async Task<Pagination<ProductBrandDto>> GetAllProductBrandsAsync(
            ProductBrandSpecParams specParams
        )
        {
            var spec = new ProductBrandSpecification(specParams);
            var brands = await _brandRepository.ListAsync(spec);
            var count = await _brandRepository.CountAsync(spec);
            return new Pagination<ProductBrandDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: brands.Adapt<IReadOnlyList<ProductBrandDto>>()
                );
        }

        public async Task<ProductBrandDto> GetProductBrandByIdAsync(Guid id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
                throw new NotFoundException(CommonMessage.NotFoundBrand);
            return brand.Adapt<ProductBrandDto>();
        }

        public async Task DeleteBrandsAsync(List<Guid> ids)
        {
            var toDelete = new List<ProductBrand>();
            foreach (var bid in ids)
            {
                var entity = await _brandRepository.GetByIdAsync(bid);
                if (entity != null)
                {
                    toDelete.Add(entity);
                }
            }
            if (toDelete.Count > 0)
            {
                await _brandRepository.DeleteRangeAsync(toDelete);
            }
        }
    }
}
