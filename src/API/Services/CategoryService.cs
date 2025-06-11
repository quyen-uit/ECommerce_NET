using AutoMapper;
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

        public async Task<bool> DeleteCategoryAsync(long id)
        {
            var existing = await _categoryRepository.GetByIdAsync(id);
            if (existing == null)
                return false;

            _categoryRepository.Delete(id);
            await _categoryRepository.Complete();
            return true;
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync(
            CategorySpecParams specParams
        )
        {
            var spec = new CategoryWithParamsAndPaginationSpec(specParams);
            var entities = await _categoryRepository.GetAllWithSpecAsync(spec);
            return entities.Adapt<IReadOnlyList<CategoryDto>>();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(long id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            return entity?.Adapt<CategoryDto>();
        }

        public async Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var existing = await _categoryRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                return null;

            existing = dto.Adapt<Category>();
            _categoryRepository.Update(existing);
            await _categoryRepository.Complete();
            return existing.Adapt<CategoryDto>();
        }
    }
}
