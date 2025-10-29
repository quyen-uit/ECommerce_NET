using API.Exceptions;
using Core.Common;
using Core.Constants;
using Core.Dtos.Categories;
using Core.Entities;
using Ardalis.Specification;
using Core.Interfaces.Services;
using Core.Specifications.Categories;
using Mapster;

namespace API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryBase<Category> _categoryRepository;

        public CategoryService(IRepositoryBase<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> AddOrUpdateCategoryAsync(CreateCategoryDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException(CommonMessage.CreateFail);
            }
            var isCreate = !dto.Id.HasValue || dto.Id == Guid.Empty;
            Category? entity = null;
            if (!isCreate)
            {
                entity = await _categoryRepository.GetByIdAsync(dto.Id!.Value);
            }
            if (entity == null)
            {
                entity = dto.Adapt<Category>();
                await _categoryRepository.AddAsync(entity);
            }
            else
            {
                dto.Adapt(entity);
                await _categoryRepository.UpdateAsync(entity);
            }
            return entity.Adapt<CategoryDto>();
        }

        public async Task<IReadOnlyList<CategoryDto>> AddRangeCategoryAsync(
            IReadOnlyList<CreateCategoryDto> dtos
        )
        {
            if (dtos == null || !dtos.Any())
            {
                throw new BadRequestException(CommonMessage.CreateFail);
            }
            var entities = dtos.Adapt<IReadOnlyList<Category>>();
            await _categoryRepository.AddRangeAsync(entities);
            return entities.Adapt<IReadOnlyList<CategoryDto>>();
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var existing = await _categoryRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundCategory);

            await _categoryRepository.DeleteAsync(existing);
        }

        public async Task<Pagination<CategoryDto>> GetAllCategoriesAsync(
            CategorySpecParams specParams
        )
        {
            var spec = new CategorySpecification(specParams);
            var entities = await _categoryRepository.ListAsync(spec);
            var specCount = new CategorySpecification(specParams, isSearch: false);
            var count = await _categoryRepository.CountAsync(specCount);
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
            var categories = await _categoryRepository.ListAsync(spec);
            var lookup = categories.ToLookup(p => p.ParentId);

            List<CategoryNodeDto> BuildTree(Guid? parentId)
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


        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException(CommonMessage.NotFoundCategory);
            return entity.Adapt<CategoryDto>();
        }
    }
}
