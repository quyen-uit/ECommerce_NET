using Core.Common;
using Core.Constants;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.ProductSkus;
using Mapster;

namespace API.Services
{
    public class ProductSkuService : IProductSkuService
    {
        private readonly IGenericRepository<ProductSku> _productSkuRepository;

        public ProductSkuService(IGenericRepository<ProductSku> productSkuRepository)
        {
            _productSkuRepository = productSkuRepository;
        }

        public async Task<ProductSkuDto> AddProductSkuAsync(CreateProductSkuDto productSkuDto)
        {
            var productSku = productSkuDto.Adapt<ProductSku>();

            _productSkuRepository.Add(productSku);
            await _productSkuRepository.Complete();

            return productSku.Adapt<ProductSkuDto>();
        }

        public async Task DeleteProductSkuAsync(long id)
        {
            var existing = await _productSkuRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundProductSku);
            _productSkuRepository.Delete(id);
            await _productSkuRepository.Complete();
        }

        public async Task<IReadOnlyList<ProductSkuDto>> GetAllProductSkusAsync(ProductSkuSpecParams productSkuSpecParams)
        {
            var spec = new ProductSkuWithColorAndSizeSpecification(productSkuSpecParams);
            var productSkus = await _productSkuRepository.GetAllWithSpecAsync(spec);
            var count = await _productSkuRepository.CountAsync(spec);
            return productSkus.Adapt<IReadOnlyList<ProductSkuDto>>();
        }

        public async Task<ProductSkuDto> GetProductSkuByIdAsync(long id)
        {
            var spec = new ProductSkuWithColorAndSizeSpecification(id);
            var productSku = await _productSkuRepository.GetAllWithSpecAsync(spec);
            if (productSku == null)
                throw new NotFoundException(CommonMessage.NotFoundProductSku);
            return productSku.Adapt<ProductSkuDto>();
        }

        public async Task<ProductSkuDto> UpdateProductSkuAsync(UpdateProductSkuDto productSkuDto)
        {
            var existing = await _productSkuRepository.GetByIdAsync(productSkuDto.Id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundProductSku);

            existing = productSkuDto.Adapt<ProductSku>();
            await _productSkuRepository.Complete();

            return existing.Adapt<ProductSkuDto>();
        }
    }

}
