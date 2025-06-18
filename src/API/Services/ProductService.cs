using Core.Common;
using Core.Constants;
using Core.Dtos;
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

        public async Task<ProductDto> AddProductAsync(CreateProductDto productDto)
        {
            var product = productDto.Adapt<Product>();

            _productRepository.Add(product);
            await _productRepository.Complete();

            return product.Adapt<ProductDto>();
        }

        // public async Task<int> CountAllProductsAsync(ProductSpecParams productSpecParams)
        // {
        //     var countSpec = new ProductsWithFiltersForCountSpecification(productSpecParams);
        //     return await _productRepository.CountAsync(countSpec);
        // }

        public async Task DeleteProductAsync(long id)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundProduct);
            _productRepository.Delete(id);
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

        public async Task<ProductDto> UpdateProductAsync(UpdateProductDto productDto)
        {
            var existing = await _productRepository.GetByIdAsync(productDto.Id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundProduct);

            existing = productDto.Adapt<Product>();
            await _productRepository.Complete();

            return existing.Adapt<ProductDto>();
        }

    }
}
