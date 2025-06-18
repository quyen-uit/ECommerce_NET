using API.Errors;
using AutoMapper;
using Core.Common;
using Core.Constants;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Categories;
using Core.Specifications.Products;
using Mapster;

namespace API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;

        public CategoryService(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> AddCategoryAsync(CreateCategoryDto dto)
        {
            var entity = dto.Adapt<Category>();
            _categoryRepository.Add(entity);
            await _categoryRepository.Complete();
            return entity.Adapt<CategoryDto>();
        }

        public async Task<IReadOnlyList<CategoryDto>> AddRangeCategoryAsync(
            IReadOnlyList<CreateCategoryDto> dtos
        )
        {
            var entities = dtos.Adapt<IReadOnlyList<Category>>();
            _categoryRepository.AddRange(entities);
            await _categoryRepository.Complete();
            return entities.Adapt<IReadOnlyList<CategoryDto>>();
        }

        public async Task DeleteCategoryAsync(long id)
        {
            var existing = await _categoryRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundCategory);

            _categoryRepository.Delete(id);
            await _categoryRepository.Complete();
        }

        public async Task<Pagination<CategoryDto>> GetAllCategoriesAsync(
            CategorySpecParams specParams
        )
        {
            var spec = new CategorySpecification(specParams);
            var entities = await _categoryRepository.GetAllWithSpecAsync(spec);
            var count = await _categoryRepository.CountAsync(spec);
            return new Pagination<CategoryDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: entities.Adapt<IReadOnlyList<CategoryDto>>()
                );
        }

        public async Task<List<CategoryNodeDto>> GetCategoriesHierarchyAsync()
        {
            var spec = new CategorySpecification(new CategorySpecParams { Sort = "order_asc" });
            var categories = await _categoryRepository.GetAllWithSpecAsync(spec);
            var lookup = categories.ToLookup(p => p.ParentId);

            List<CategoryNodeDto> BuildTree(long? parentId)
            {
                return lookup[parentId].Select(p => new CategoryNodeDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ChildCategories = BuildTree(p.Id)
                })
                .ToList();
            }

            return BuildTree(null); // Start from root nodes (ParentId == null)
        }


        public async Task<CategoryDto> GetCategoryByIdAsync(long id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException(CommonMessage.NotFoundCategory);
            return entity.Adapt<CategoryDto>();
        }

        public async Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var existing = await _categoryRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundCategory);

            existing = dto.Adapt<Category>();
            _categoryRepository.Update(existing);
            await _categoryRepository.Complete();
            return existing.Adapt<CategoryDto>();
        }
    }
}
