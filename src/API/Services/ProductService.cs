using Core.Common;
using Core.Constants;
using Core.Dtos.Products;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Products;
using Mapster;

namespace API.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductService(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> AddOrUpdateProductAsync(CreateProductDto dto)
        {
            var entity = await _productRepository.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                entity = dto.Adapt<Product>();
                _productRepository.Add(entity);
            }
            else
            {
                dto.Adapt(entity);
                _productRepository.Update(entity);
            }
            await _productRepository.Complete();
            return entity.Adapt<ProductDto>();
        }

        public async Task DeleteProductAsync(long id)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundProduct);
            _productRepository.Delete(existing);
            await _productRepository.Complete();
        }

        public async Task<Pagination<ProductDto>> GetAllProductFilterByNameAsync(ProductFilterByNameSpecParams productSpecParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productSpecParams);
            var products = await _productRepository.GetAllWithSpecAsync(spec);
            var count = await _productRepository.CountAsync(spec);
            return new Pagination<ProductDto>(
                    pageNumber: productSpecParams.PageNumber,
                    pageSize: productSpecParams.PageSize,
                    pageCount: count,
                    data: products.Adapt<IReadOnlyList<ProductDto>>()
                );
        }

        public async Task<Pagination<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpecParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productSpecParams);
            var products = await _productRepository.GetAllWithSpecAsync(spec);
            var count = await _productRepository.CountAsync(spec);
            return new Pagination<ProductDto>(
                    pageNumber: productSpecParams.PageNumber,
                    pageSize: productSpecParams.PageSize,
                    pageCount: count,
                    data: products.Adapt<IReadOnlyList<ProductDto>>()
                );
        }

        public async Task<ProductDto> GetProductByIdAsync(long id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);
            var product = await _productRepository.GetEntityWithSpecAsync(spec);
            if (product == null)
                throw new NotFoundException(CommonMessage.NotFoundProduct);
            return product.Adapt<ProductDto>();
        }

    }
}
