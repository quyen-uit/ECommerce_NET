using Core.Exceptions;
using Core.Common;
using Core.Constants;
using Core.Dtos.Categories;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Specifications.Categories;
using Core.Interfaces.Reposiories;
using Core.Interfaces;
using Mapster;

namespace API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> AddOrUpdateCategoryAsync(CreateCategoryDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException(CommonMessage.CreateFail);
            }
            var categoryRepo = _unitOfWork.Repository<Category>();
            var isCreate = !dto.Id.HasValue || dto.Id == Guid.Empty;
            Category? entity = null;
            if (!isCreate)
            {
                entity = await categoryRepo.GetByIdAsync(dto.Id!.Value);
            }
            if (entity == null)
            {
                entity = dto.Adapt<Category>();
                await categoryRepo.AddAsync(entity);
            }
            else
            {
                dto.Adapt(entity);
                await categoryRepo.UpdateAsync(entity);
            }
            await _unitOfWork.SaveChangesAsync();
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
            var categoryRepo = _unitOfWork.Repository<Category>();
            var entities = dtos.Adapt<IReadOnlyList<Category>>();
            await categoryRepo.AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return entities.Adapt<IReadOnlyList<CategoryDto>>();
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var categoryRepo = _unitOfWork.Repository<Category>();
            var existing = await categoryRepo.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundCategory);

            await categoryRepo.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Pagination<CategoryDto>> GetAllCategoriesAsync(
            CategorySpecParams specParams
        )
        {
            var categoryRepo = _unitOfWork.Repository<Category>();
            var spec = new CategorySpecification(specParams);
            var entities = await categoryRepo.ListAsync(spec);
            var specCount = new CategorySpecification(specParams, isSearch: false);
            var count = await categoryRepo.CountAsync(specCount);
            return new Pagination<CategoryDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: entities.Adapt<IReadOnlyList<CategoryDto>>()
                );
        }

        public async Task<List<CategoryNodeDto>> GetCategoriesHierarchyAsync()
        {
            var categoryRepo = _unitOfWork.Repository<Category>();
            var spec = new CategorySpecification(new CategorySpecParams { Sort = "order_asc" });
            var categories = await categoryRepo.ListAsync(spec);
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
            var categoryRepo = _unitOfWork.Repository<Category>();
            var entity = await categoryRepo.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException(CommonMessage.NotFoundCategory);
            return entity.Adapt<CategoryDto>();
        }
    }
}
