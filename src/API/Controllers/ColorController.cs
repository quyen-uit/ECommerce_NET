using API.Errors;
using API.Helpers;
using Core.Dtos;
using Core.Interfaces.Services;
using Core.Specifications.Colors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ColorController : ApiControllerBase
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        // ✅ GET: api/color/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ColorDto>> GetColor(long id)
        {
            var colorDto = await _colorService.GetColorByIdAsync(id);
            if (colorDto == null)
                return NotFound(new ApiException(404, "Color not found"));

            return Ok(colorDto);
        }

        // 🔄 POST: api/color/get-all
        [HttpPost("get-all")]
        public async Task<ActionResult<Pagination<ColorDto>>> GetColors(
            [FromBody] ColorSpecParams specParams
        )
        {
            var colorDtos = await _colorService.GetAllColorsAsync(specParams);

            return Ok(
                new Pagination<ColorDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: colorDtos.Count,
                    data: colorDtos
                )
            );
        }

        // ✅ POST: api/color/create
        [HttpPost("create")]
        public async Task<ActionResult<ColorDto>> CreateColor([FromBody] CreateColorDto colorDto)
        {
            var result = await _colorService.AddColorAsync(colorDto);
            if (result == null)
                return BadRequest(new ApiResponse(400, "Failed to create color"));

            return CreatedAtAction(nameof(GetColor), new { id = result.Id }, result);
        }

        // ✅ POST: api/color/create-many
        [HttpPost("create-many")]
        public async Task<ActionResult<IReadOnlyList<ColorDto>>> CreateColors(
            [FromBody] IReadOnlyList<CreateColorDto> colorDtos
        )
        {
            var result = await _colorService.AddRangeColorAsync(colorDtos);
            if (result == null || !result.Any())
                return BadRequest(new ApiResponse(400, "Failed to create colors"));

            return Ok(result);
        }

        // ✅ POST: api/color/update
        [HttpPost("update")]
        public async Task<ActionResult<ColorDto>> UpdateColor([FromBody] UpdateColorDto colorDto)
        {
            var result = await _colorService.UpdateColorAsync(colorDto);
            if (result == null)
                return NotFound(new ApiException(404, "Color not found"));

            return Ok(result);
        }

        // ✅ DELETE: api/color/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteColor(long id)
        {
            var success = await _colorService.DeleteColorAsync(id);
            if (!success)
                return NotFound(new ApiException(404, "Color not found"));

            return Ok(new ApiResponse(200, "Color deleted successfully"));
        }
    }
}
