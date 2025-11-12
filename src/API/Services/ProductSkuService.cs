using Core.Constants;
using Core.Exceptions;
using Core.Common;
using Core.Dtos.ProductSkus;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Specifications.ProductSkus;
using Core.Interfaces.Reposiories;
using Core.Interfaces;
using Mapster;

namespace API.Services
{
    public class ProductSkuService : IProductSkuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public ProductSkuService(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<ProductSkuDto> AddOrUpdateProductSkuAsync(CreateProductSkuDto dto)
        {
            var productSkuRepo = _unitOfWork.Repository<ProductSku>();
            ProductSku? entity = null;
            if (dto.Id.HasValue && dto.Id != Guid.Empty)
            {
                entity = await productSkuRepo.GetByIdAsync(dto.Id.Value);
            }
            if (entity == null)
            {
                entity = dto.Adapt<ProductSku>();
                await productSkuRepo.AddAsync(entity);
            }
            else
            {
                dto.Adapt(entity);
                await productSkuRepo.UpdateAsync(entity);
            }
            await _unitOfWork.SaveChangesAsync();
            return entity.Adapt<ProductSkuDto>();
        }

        public async Task DeleteProductSkuAsync(Guid id)
        {
            var productSkuRepo = _unitOfWork.Repository<ProductSku>();
            var existing = await productSkuRepo.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundProductSku);
            await productSkuRepo.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Pagination<ProductSkuDto>> GetAllProductSkusAsync(ProductSkuSpecParams productSkuSpecParams)
        {
            var productSkuRepo = _unitOfWork.Repository<ProductSku>();
            var spec = new ProductSkuWithColorAndSizeSpecification(productSkuSpecParams);
            var productSkus = await productSkuRepo.ListAsync(spec);
            var specCount = new ProductSkuWithColorAndSizeSpecification(productSkuSpecParams, isSearch: false);
            var count = await productSkuRepo.CountAsync(specCount);
            return new Pagination<ProductSkuDto>(
                pageNumber: productSkuSpecParams.PageNumber,
                pageSize: productSkuSpecParams.PageSize,
                pageCount: count,
                data: productSkus.Adapt<IReadOnlyList<ProductSkuDto>>()
            );
        }

        public async Task<ProductSkuDto> GetProductSkuByIdAsync(Guid id)
        {
            var productSkuRepo = _unitOfWork.Repository<ProductSku>();
            var spec = new ProductSkuWithColorAndSizeSpecification(id);
            var productSku = await productSkuRepo.FirstOrDefaultAsync(spec);
            if (productSku == null)
                throw new NotFoundException(CommonMessage.NotFoundProductSku);

            var sku = productSku.Adapt<ProductSkuDto>();
            sku.Images = await _imageService.GetAllImageByRefIdAsync(id, ImageType.Sku);
            return sku;
        }
    }

}
