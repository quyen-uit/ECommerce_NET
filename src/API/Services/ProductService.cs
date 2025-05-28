using AutoMapper;
using Core.Dtos.CreateDto;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Products;

namespace API.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Product> AddProductAsync(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            _productRepository.Add(product);
            await _productRepository.Complete();

            return await GetProductByIdAsync(product.Id);
        }

        public async Task<int> CountAllProductsAsync(ProductSpecParams productSpecParams)
        {
            var countSpec = new ProductsWithFiltersForCountSpecification(productSpecParams);
            return await _productRepository.CountAsync(countSpec);
        }

        public async Task<long> DeleteProductAsync(long id)
        {
            _productRepository.Delete(id);
            return await _productRepository.Complete();
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync(ProductSpecParams productSpecParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productSpecParams);
            var products = await _productRepository.GetAllWithSpecAsync(spec);
            return products;
        }

        public async Task<Product> GetProductByIdAsync(long id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);
            var products = await _productRepository.GetEntityWithSpecAsync(spec);
            return products;
        }

        public async Task<Product> UpdateProductAsync(long id, CreateProductDto productDto)
        {

            var product = _mapper.Map<Product>(productDto);
            product.Id = id;

            _productRepository.Update(product);
            await _productRepository.Complete();

            return await GetProductByIdAsync(id);
        }

    }
}
