using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
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
            if (dto == null)
                return NotFound(new ApiException(404, "Category not found"));

            return Ok(dto);
        }

        [HttpPost("get-all")]
        public async Task<ActionResult<Pagination<CategoryDto>>> GetCategories(
            [FromBody] CategorySpecParams specParams
        )
        {
            var dtos = await _categoryService.GetAllCategoriesAsync(specParams);
            return Ok(
                new Pagination<CategoryDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: dtos.Count,
                    data: dtos
                )
            );
        }

        [HttpPost("create")]
        public async Task<ActionResult<CategoryDto>> CreateCategory(
            [FromBody] CreateCategoryDto dto
        )
        {
            var result = await _categoryService.AddCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategory), new { id = result.Id }, result);
        }

        [HttpPost("create-many")]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> CreateCategories(
            [FromBody] IReadOnlyList<CreateCategoryDto> dtos
        )
        {
            var result = await _categoryService.AddRangeCategoryAsync(dtos);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(
            [FromBody] UpdateCategoryDto dto
        )
        {
            var result = await _categoryService.UpdateCategoryAsync(dto);
            if (result == null)
                return NotFound(new ApiException(404, "Category not found"));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(long id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success)
                return NotFound(new ApiException(404, "Category not found"));

            return Ok(new ApiResponse(200, "Category deleted successfully"));
        }
    }
}
