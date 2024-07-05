using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoryController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories([FromQuery] bool? isActive)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(isActive);
            var categoryDtos = _mapper.Map<IReadOnlyList<CategoryDto>>(categories);

            return Ok(categoryDtos);
        }


        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(CreateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            var result = await _categoryService.AddCategoryAsync(category);

            if (result < 0)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<CategoryDto>(category));
        }

        // POST: api/Category/many
        [HttpPost("many")]
        public async Task<ActionResult<CategoryDto>> PostCategories(IReadOnlyList<CreateCategoryDto> categoryDtos)
        {
            var categories = _mapper.Map<IReadOnlyList<Category>>(categoryDtos);
            var result = await _categoryService.AddRangeCategoryAsync(categories);

            if (result < 0)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<IReadOnlyList<CategoryDto>>(categories));
        }

        // PUT: api/Category/5
        [HttpPut]
        public async Task<IActionResult> PutCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            var result = await _categoryService.UpdateCategoryAsync(category);

            if (result < 0)
            {
                return BadRequest(new ApiResponse(400, "Updating fail"));
            }
            return Ok(_mapper.Map<CategoryDto>(category));
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Not found category");
            }

            var result = await _categoryService.DeleteCategoryAsync(id);

            if (result < 0)
            {
                return BadRequest(new ApiResponse(400, "Deleting fail"));
            }
            return Ok("Delete succesfully");
        }
    }
}
