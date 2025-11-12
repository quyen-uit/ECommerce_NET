using Core.Common;
using Core.Exceptions;
using Core.Constants;
using Core.Dtos.ProductBrands;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Specifications.ProductBrands;
using Core.Interfaces.Reposiories;
using Core.Interfaces;
using Mapster;

namespace API.Services
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductBrandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductBrandDto> AddOrUpdateProductBrandAsync(CreateProductBrandDto dto)
        {
            var brandRepo = _unitOfWork.Repository<ProductBrand>();
            ProductBrand? entity = null;
            if (dto.Id.HasValue && dto.Id != Guid.Empty)
            {
                entity = await brandRepo.GetByIdAsync(dto.Id.Value);
            }
            if (entity == null)
            {
                entity = dto.Adapt<ProductBrand>();
                await brandRepo.AddAsync(entity);
            }
            else
            {
                dto.Adapt(entity);
                await brandRepo.UpdateAsync(entity);
            }
            await _unitOfWork.SaveChangesAsync();
            return entity.Adapt<ProductBrandDto>();
        }

        public async Task<IReadOnlyList<ProductBrandDto>> AddRangeProductBrandAsync(
            IReadOnlyList<CreateProductBrandDto> brandDtos
        )
        {
            var brandRepo = _unitOfWork.Repository<ProductBrand>();
            var brands = brandDtos.Adapt<IReadOnlyList<ProductBrand>>();
            await brandRepo.AddRangeAsync(brands);
            await _unitOfWork.SaveChangesAsync();
            return brands.Adapt<IReadOnlyList<ProductBrandDto>>();
        }

        public async Task<int> CountAllAsync(ProductBrandSpecParams specParams)
        {
            var brandRepo = _unitOfWork.Repository<ProductBrand>();
            var spec = new ProductBrandSpecification(specParams);
            return await brandRepo.CountAsync(spec);
        }

        public async Task DeleteProductBrandAsync(Guid id)
        {
            var brandRepo = _unitOfWork.Repository<ProductBrand>();
            var existing = await brandRepo.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundBrand);

            await brandRepo.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Pagination<ProductBrandDto>> GetAllProductBrandsAsync(
            ProductBrandSpecParams specParams
        )
        {
            var brandRepo = _unitOfWork.Repository<ProductBrand>();
            var spec = new ProductBrandSpecification(specParams);
            var brands = await brandRepo.ListAsync(spec);
            var count = await brandRepo.CountAsync(spec);
            return new Pagination<ProductBrandDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: brands.Adapt<IReadOnlyList<ProductBrandDto>>()
                );
        }

        public async Task<ProductBrandDto> GetProductBrandByIdAsync(Guid id)
        {
            var brandRepo = _unitOfWork.Repository<ProductBrand>();
            var brand = await brandRepo.GetByIdAsync(id);
            if (brand == null)
                throw new NotFoundException(CommonMessage.NotFoundBrand);
            return brand.Adapt<ProductBrandDto>();
        }

        public async Task DeleteBrandsAsync(List<Guid> ids)
        {
            var brandRepo = _unitOfWork.Repository<ProductBrand>();
            var toDelete = new List<ProductBrand>();
            foreach (var bid in ids)
            {
                var entity = await brandRepo.GetByIdAsync(bid);
                if (entity != null)
                {
                    toDelete.Add(entity);
                }
            }
            if (toDelete.Count > 0)
            {
                await brandRepo.DeleteRangeAsync(toDelete);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
