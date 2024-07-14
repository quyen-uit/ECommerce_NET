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
        private readonly IGenericRepository<ProductColor> _productColorRepository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<ProductColor> productColorRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _productColorRepository = productColorRepository;
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

        public async Task<int> DeleteProductAsync(int id)
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

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);
            var products = await _productRepository.GetEntityWithSpecAsync(spec);
            return products;
        }

        public async Task<Product> UpdateProductAsync(int id, CreateProductDto productDto)
        {

            var product = _mapper.Map<Product>(productDto);
            product.Id = id;

            _productRepository.Update(product);
            await _productRepository.Complete();

            return await GetProductByIdAsync(id);
        }

        // product color
        public async Task<ProductColor> AddProductColorAsync(CreateProductColorDto productColorDto)
        {
            if (await CheckExistColorInProduct(productColorDto.ProductId, productColorDto.ColorId))
            {
                throw new Exception("Color existed in this product");
            }

            var productColor = _mapper.Map<ProductColor>(productColorDto);

            _productColorRepository.Add(productColor);
            await _productColorRepository.Complete();

            return await GetProductColorsByIdAsync(productColor.Id);
        }

        public async Task<ProductColor> UpdateProductColorAsync(int id, CreateProductColorDto productColorDto)
        {
            var productColor = _mapper.Map<ProductColor>(productColorDto);
            productColor.Id = id;

            _productColorRepository.Update(productColor);
            await _productColorRepository.Complete();

            return await GetProductColorsByIdAsync(id);
        }

        public async Task<ProductColor> GetProductColorsByIdAsync(int productId)
        {
            var productColor = await _productColorRepository.GetByIdAsync(productId);
            return productColor;
        }

        public async Task<int> DeleteProductColorAsync(int id)
        {
            _productColorRepository.Delete(id);
            return await _productColorRepository.Complete();
        }

        private async Task<bool> CheckExistColorInProduct(int productId, int colorId)
        {
            var spec = new ProductColorSpecification(productId, colorId);
            var existProductColor = await _productColorRepository.GetEntityWithSpecAsync(spec);
            return existProductColor != null;
        }
    }
}
