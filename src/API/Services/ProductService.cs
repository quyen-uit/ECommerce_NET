using Core.Common;
using Core.Exceptions;
using Core.Constants;
using Core.Dtos.Products;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Specifications.Products;
using Core.Interfaces.Reposiories;
using Mapster;

namespace API.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDto> AddOrUpdateProductAsync(CreateProductDto dto)
        {
            var productRepo = _unitOfWork.Repository<Product>();
            Product? entity = null;
            if (dto.Id.HasValue && dto.Id != Guid.Empty)
            {
                entity = await productRepo.GetByIdAsync(dto.Id.Value);
            }
            if (entity == null)
            {
                entity = dto.Adapt<Product>();
                await productRepo.AddAsync(entity);
            }
            else
            {
                dto.Adapt(entity);
                await productRepo.UpdateAsync(entity);
            }
            await _unitOfWork.SaveChangesAsync();
            return entity.Adapt<ProductDto>();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var productRepo = _unitOfWork.Repository<Product>();
            var existing = await productRepo.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundProduct);
            await productRepo.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Pagination<ProductDto>> GetAllProductFilterByNameAsync(ProductFilterByNameSpecParams productSpecParams)
        {
            var productRepo = _unitOfWork.Repository<Product>();
            var spec = new ProductWithTypesAndBrandsSpecification(productSpecParams);
            var products = await productRepo.ListAsync(spec);
            var specCount = new ProductWithTypesAndBrandsSpecification(productSpecParams, isSearch: false);
            var count = await productRepo.CountAsync(specCount);
            return new Pagination<ProductDto>(
                    pageNumber: productSpecParams.PageNumber,
                    pageSize: productSpecParams.PageSize,
                    pageCount: count,
                    data: products.Adapt<IReadOnlyList<ProductDto>>()
                );
        }

        public async Task<Pagination<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpecParams)
        {
            var productRepo = _unitOfWork.Repository<Product>();
            var spec = new ProductWithTypesAndBrandsSpecification(productSpecParams);
            var products = await productRepo.ListAsync(spec);
            var specCount = new ProductWithTypesAndBrandsSpecification(productSpecParams, isSearch: false);
            var count = await productRepo.CountAsync(specCount);
            return new Pagination<ProductDto>(
                    pageNumber: productSpecParams.PageNumber,
                    pageSize: productSpecParams.PageSize,
                    pageCount: count,
                    data: products.Adapt<IReadOnlyList<ProductDto>>()
                );
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var productRepo = _unitOfWork.Repository<Product>();
            var spec = new ProductWithTypesAndBrandsSpecification(id);
            var product = await productRepo.FirstOrDefaultAsync(spec);
            if (product == null)
                throw new NotFoundException(CommonMessage.NotFoundProduct);
            return product.Adapt<ProductDto>();
        }

    }
}
