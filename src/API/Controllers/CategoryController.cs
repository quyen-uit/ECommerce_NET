using API.Errors;
using Core.Common;
using Core.Constants;
using Core.Dtos;
using Core.Interfaces.Services;
using Core.Specifications.Categories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoryController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(long id)
        {
            var dto = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(dto);
        }

        [HttpPost("get-all")]
        public async Task<ActionResult<Pagination<CategoryDto>>> GetCategories(
            [FromBody] CategorySpecParams specParams
        )
        {
            var result = await _categoryService.GetAllCategoriesAsync(specParams);
            return Ok(result);
        }

        [HttpGet("hierarchy")]
        public async Task<ActionResult<List<CategoryNodeDto>>> GetHierarchyCategories()
        {
            var result = await _categoryService.GetCategoriesHierarchyAsync();
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<CategoryDto>> CreateCategory(
            [FromBody] CreateCategoryDto dto
        )
        {
            var result = await _categoryService.AddCategoryAsync(dto);
            if (result == null)
                return BadRequest(new ApiResponse(400, CommonMessage.CreateFail));

            return Ok(result);
        }

        [HttpPost("create-many")]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> CreateCategories(
            [FromBody] IReadOnlyList<CreateCategoryDto> dtos
        )
        {
            var result = await _categoryService.AddRangeCategoryAsync(dtos);
            if (result == null || !result.Any())
                return BadRequest(new ApiResponse(400, CommonMessage.CreateFail));
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(
            [FromBody] UpdateCategoryDto dto
        )
        {
            var result = await _categoryService.UpdateCategoryAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(long id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok(CommonMessage.DeleteSuccess);
        }
    }
}
