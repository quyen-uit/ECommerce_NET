using Core.Common;
using Core.Exceptions;
using Core.Constants;
using Core.Dtos.Products;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Specifications.Products;
using Core.Interfaces.Reposiories;
using Mapster;

namespace API.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> AddOrUpdateProductAsync(CreateProductDto dto)
        {
            Product? entity = null;
            if (dto.Id.HasValue && dto.Id != Guid.Empty)
            {
                entity = await _productRepository.GetByIdAsync(dto.Id.Value);
            }
            if (entity == null)
            {
                entity = dto.Adapt<Product>();
                await _productRepository.AddAsync(entity);
            }
            else
            {
                dto.Adapt(entity);
                await _productRepository.UpdateAsync(entity);
            }
            return entity.Adapt<ProductDto>();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundProduct);
            await _productRepository.DeleteAsync(existing);
        }

        public async Task<Pagination<ProductDto>> GetAllProductFilterByNameAsync(ProductFilterByNameSpecParams productSpecParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productSpecParams);
            var products = await _productRepository.ListAsync(spec);
            var specCount = new ProductWithTypesAndBrandsSpecification(productSpecParams, isSearch: false);
            var count = await _productRepository.CountAsync(specCount);
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
            var products = await _productRepository.ListAsync(spec);
            var specCount = new ProductWithTypesAndBrandsSpecification(productSpecParams, isSearch: false);
            var count = await _productRepository.CountAsync(specCount);
            return new Pagination<ProductDto>(
                    pageNumber: productSpecParams.PageNumber,
                    pageSize: productSpecParams.PageSize,
                    pageCount: count,
                    data: products.Adapt<IReadOnlyList<ProductDto>>()
                );
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);
            var product = await _productRepository.FirstOrDefaultAsync(spec);
            if (product == null)
                throw new NotFoundException(CommonMessage.NotFoundProduct);
            return product.Adapt<ProductDto>();
        }

    }
}
