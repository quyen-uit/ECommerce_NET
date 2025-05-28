using AutoMapper;
using Core.Dtos;
using Core.Dtos.CreateDto;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Categories;
using Core.Specifications.Products;

namespace API.Services
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IMapper _mapper;

        public ProductBrandService(IGenericRepository<ProductBrand> productBrandRepository, IMapper mapper)
        {
            _productBrandRepository = productBrandRepository;
            _mapper = mapper;
        }
        public async Task<ProductBrand> AddProductBrandAsync(CreateProductBrandDto productBrandDto)
        {
            var productBrand = _mapper.Map<ProductBrand>(productBrandDto);

            _productBrandRepository.Add(productBrand);
            await _productBrandRepository.Complete();

            return productBrand;
        }

        public async Task<long> DeleteProductBrandAsync(long id)
        {
            _productBrandRepository.Delete(id);
            return await _productBrandRepository.Complete();
        }

        public async Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync(ProductBrandSpecParams specParams)
        {
            var spec = new ProductBrandWithParamsAndPaginationSpec(specParams);

            return await _productBrandRepository.GetAllWithSpecAsync(spec);
        }

        public async Task<ProductBrand> UpdateProductBrandAsync(long id, CreateProductBrandDto productBrandDto)
        {
            var productBrand = _mapper.Map<ProductBrand>(productBrandDto);
            productBrand.Id = id;

            _productBrandRepository.Update(productBrand);
            await _productBrandRepository.Complete();
            return productBrand;
        }

        public async Task<IReadOnlyList<ProductBrand>> AddRangeProductBrandAsync(IReadOnlyList<CreateProductBrandDto> productBrandDtos)
        {
            var productBrands = _mapper.Map<IReadOnlyList<ProductBrand>>(productBrandDtos);

            _productBrandRepository.AddRange(productBrands);
            await _productBrandRepository.Complete();
            return productBrands;
        }

        public async Task<ProductBrand> GetProductBrandByIdAsync(long id)
        {
            return await _productBrandRepository.GetByIdAsync(id);
        }

        public async Task<int> CountAllAsync(ProductBrandSpecParams specParams)
        {
            var countSpec = new ProductBrandWithParamsSpec(specParams);
            return await _productBrandRepository.CountAsync(countSpec);
        }
    }
}
