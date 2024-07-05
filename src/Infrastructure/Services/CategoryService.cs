using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Products;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;

        public CategoryService(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<int> AddCategoryAsync(Category category)
        {
            _categoryRepository.Add(category);
            return await _categoryRepository.Complete();
        }

        public async Task<int> AddRangeCategoryAsync(IReadOnlyList<Category> categories)
        {
            _categoryRepository.AddRange(categories);
            return await _categoryRepository.Complete();
        }


        public async Task<int> DeleteCategoryAsync(int id)
        {
            _categoryRepository.Delete(id);
            return await _categoryRepository.Complete();
        }

        public async Task<IReadOnlyList<Category>> GetAllCategoriesAsync(bool? isActive)
        {
            if (isActive != null)
            {
                var spec = new ProductTypesWithActiveFilterSpecification((bool)isActive);
                return await _categoryRepository.GetAllWithSpecAsync(spec);
            }
            return await _categoryRepository.GetAllAsync();

        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<int> UpdateCategoryAsync(Category category)
        {
            _categoryRepository.Update(category);
            return await _categoryRepository.Complete();
        }
    }
}
