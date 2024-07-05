using Core.Dtos;
using Core.Dtos.CreateDto;
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
        [HttpGet("all")]
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
            var result = await _categoryService.AddCategoryAsync(categoryDto);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<CategoryDto>(result));
        }

        // POST: api/Category/many
        [HttpPost("many")]
        public async Task<ActionResult<CategoryDto>> PostCategories(IReadOnlyList<CreateCategoryDto> categoryDtos)
        {
            var result = await _categoryService.AddRangeCategoryAsync(categoryDtos);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<IReadOnlyList<CategoryDto>>(result));
        }

        // PUT: api/Category/5
        [HttpPut]
        public async Task<IActionResult> PutCategory(CategoryDto categoryDto)
        {
            var result = await _categoryService.UpdateCategoryAsync(categoryDto);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Updating fail"));
            }
            return Ok(_mapper.Map<CategoryDto>(result));
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
