using AutoMapper;
using Core.Dtos;
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

        public Task<int> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync(ProductSpecParams productSpecParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productSpecParams);
            var products = await _productRepository.GetAllWithSpecAsync(spec);
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);
            var products = await _productRepository.GetEntityWithSpecAsync(spec);
            return products;
        }

        public Task<int> UpdateProductAsync(int id, CreateProductDto product)
        {
            throw new NotImplementedException();
        }
    }
}
