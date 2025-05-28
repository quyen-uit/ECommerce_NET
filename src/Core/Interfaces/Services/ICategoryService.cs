using Core.Dtos;
using Core.Dtos.CreateDto;
using Core.Entities;
using Core.Specifications.Categories;

namespace Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetAllCategoriesAsync(CategorySpecParams specParams);
        Task<Category> GetCategoryByIdAsync(long id);
        Task<Category> AddCategoryAsync(CreateCategoryDto category);
        Task<IReadOnlyList<Category>> AddRangeCategoryAsync(IReadOnlyList<CreateCategoryDto> categories);
        Task<Category> UpdateCategoryAsync(long id, CreateCategoryDto category);
        Task<long> DeleteCategoryAsync(long id);
    }

}
