using Core.Constants;
using Core.Exceptions;
using Core.Common;
using Core.Dtos.ProductSkus;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Specifications.ProductSkus;
using Core.Interfaces.Reposiories;
using Mapster;

namespace API.Services
{
    public class ProductSkuService : IProductSkuService
    {
        private readonly IRepository<ProductSku> _productSkuRepository;
        private readonly IImageService _imageService;

        public ProductSkuService(IRepository<ProductSku> productSkuRepository, IImageService imageService)
        {
            _productSkuRepository = productSkuRepository;
            _imageService = imageService;
        }

        public async Task<ProductSkuDto> AddOrUpdateProductSkuAsync(CreateProductSkuDto dto)
        {
            ProductSku? entity = null;
            if (dto.Id.HasValue && dto.Id != Guid.Empty)
            {
                entity = await _productSkuRepository.GetByIdAsync(dto.Id.Value);
            }
            if (entity == null)
            {
                entity = dto.Adapt<ProductSku>();
                await _productSkuRepository.AddAsync(entity);
            }
            else
            {
                dto.Adapt(entity);
                await _productSkuRepository.UpdateAsync(entity);
            }
            return entity.Adapt<ProductSkuDto>();
        }

        public async Task DeleteProductSkuAsync(Guid id)
        {
            var existing = await _productSkuRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundProductSku);
            await _productSkuRepository.DeleteAsync(existing);
        }

        public async Task<Pagination<ProductSkuDto>> GetAllProductSkusAsync(ProductSkuSpecParams productSkuSpecParams)
        {
            var spec = new ProductSkuWithColorAndSizeSpecification(productSkuSpecParams);
            var productSkus = await _productSkuRepository.ListAsync(spec);
            var specCount = new ProductSkuWithColorAndSizeSpecification(productSkuSpecParams, isSearch: false);
            var count = await _productSkuRepository.CountAsync(specCount);
            return new Pagination<ProductSkuDto>(
                pageNumber: productSkuSpecParams.PageNumber,
                pageSize: productSkuSpecParams.PageSize,
                pageCount: count,
                data: productSkus.Adapt<IReadOnlyList<ProductSkuDto>>()
            );
        }

        public async Task<ProductSkuDto> GetProductSkuByIdAsync(Guid id)
        {
            var spec = new ProductSkuWithColorAndSizeSpecification(id);
            var productSku = await _productSkuRepository.FirstOrDefaultAsync(spec);
            if (productSku == null)
                throw new NotFoundException(CommonMessage.NotFoundProductSku);

            var sku = productSku.Adapt<ProductSkuDto>();
            sku.Images = await _imageService.GetAllImageByRefIdAsync(id, ImageType.Sku);
            return sku;
        }
    }

}
