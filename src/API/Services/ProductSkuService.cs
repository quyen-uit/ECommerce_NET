using Core.Constants;
using Core.Dtos.ProductSkus;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.ProductSkus;
using Mapster;

namespace API.Services
{
    public class ProductSkuService : IProductSkuService
    {
        private readonly IGenericRepository<ProductSku> _productSkuRepository;
        private readonly IImageService _imageService;

        public ProductSkuService(IGenericRepository<ProductSku> productSkuRepository, IImageService imageService)
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
                _productSkuRepository.Add(entity);
            }
            else
            {
                dto.Adapt(entity);
                _productSkuRepository.Update(entity);
            }
            await _productSkuRepository.Complete();
            return entity.Adapt<ProductSkuDto>();
        }

        public async Task DeleteProductSkuAsync(Guid id)
        {
            var existing = await _productSkuRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundProductSku);
            _productSkuRepository.Delete(existing);
            await _productSkuRepository.Complete();
        }

        public async Task<IReadOnlyList<ProductSkuDto>> GetAllProductSkusAsync(ProductSkuSpecParams productSkuSpecParams)
        {
            var spec = new ProductSkuWithColorAndSizeSpecification(productSkuSpecParams);
            var productSkus = await _productSkuRepository.GetAllWithSpecAsync(spec);
            var count = await _productSkuRepository.CountAsync(spec);
            return productSkus.Adapt<IReadOnlyList<ProductSkuDto>>();
        }

        public async Task<ProductSkuDto> GetProductSkuByIdAsync(Guid id)
        {
            var spec = new ProductSkuWithColorAndSizeSpecification(id);
            var productSku = await _productSkuRepository.GetEntityWithSpecAsync(spec);
            if (productSku == null)
                throw new NotFoundException(CommonMessage.NotFoundProductSku);

            var sku = productSku.Adapt<ProductSkuDto>();
            sku.Images = await _imageService.GetAllImageByRefIdAsync(id, ImageType.Sku);
            return sku;
        }
    }

}
