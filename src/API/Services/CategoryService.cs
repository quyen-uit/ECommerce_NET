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
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Category> AddCategoryAsync(CreateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            _categoryRepository.Add(category);
            await _categoryRepository.Complete();

            return category;
        }

        public async Task<IReadOnlyList<Category>> AddRangeCategoryAsync(IReadOnlyList<CreateCategoryDto> categoryDtos)
        {
            var categories = _mapper.Map<IReadOnlyList<Category>>(categoryDtos);

            _categoryRepository.AddRange(categories);
            await _categoryRepository.Complete();

            return categories;
        }


        public async Task<int> DeleteCategoryAsync(int id)
        {
            _categoryRepository.Delete(id);
            return await _categoryRepository.Complete();
        }

        public async Task<IReadOnlyList<Category>> GetAllCategoriesAsync(CategorySpecParams specParams)
        {
            var spec = new CategoryWithParamsSpec(specParams);
            return await _categoryRepository.GetAllWithSpecAsync(spec);
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> UpdateCategoryAsync(int id, CreateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = id;

            _categoryRepository.Update(category);
            await _categoryRepository.Complete();
            return category;

        }
    }
}
