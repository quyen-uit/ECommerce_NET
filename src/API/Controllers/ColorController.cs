using API.Commons.Response;
using API.Helpers;
using Core.Common;
using Core.Dtos.Colors;
using Core.Interfaces.Services;
using Core.Specifications.Colors;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "Permission:Color.Read")]
        public async Task<ActionResult<ApiSuccessResponse<ColorDto>>> GetColor(Guid id)
        {
            var colorDto = await _colorService.GetColorByIdAsync(id);
            return Ok(ResponseFactory.Ok(colorDto));
        }

        // 🔄 POST: api/color/get-all
        [HttpPost("search")]
        [Authorize(Policy = "Permission:Color.Read")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<ColorDto>>>> GetColors(
                    [FromBody] ColorSpecParams specParams
                )
        {
            var result = await _colorService.GetAllColorsAsync(specParams);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/color/create
        [HttpPost("create")]
        [Authorize(Policy = "Permission:Color.Create")]
        public async Task<ActionResult<ApiSuccessResponse<ColorDto>>> CreateColor([FromBody] CreateColorDto colorDto)
        {
            var result = await _colorService.AddOrUpdateColorAsync(colorDto);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/color/create-many
        [HttpPost("create-many")]
        [Authorize(Policy = "Permission:Color.Create")]
        public async Task<ActionResult<ApiSuccessResponse<IReadOnlyList<ColorDto>>>> CreateColors(
            [FromBody] IReadOnlyList<CreateColorDto> colorDtos
        )
        {
            var result = await _colorService.AddRangeColorAsync(colorDtos);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/color/update
        [HttpPost("update")]
        [Authorize(Policy = "Permission:Color.Update")]
        public async Task<ActionResult<ApiSuccessResponse<ColorDto>>> UpdateColor([FromBody] CreateColorDto colorDto)
        {
            var result = await _colorService.AddOrUpdateColorAsync(colorDto);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ DELETE: api/color/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "Permission:Color.Delete")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteColor(Guid id)
        {
            await _colorService.DeleteColorAsync(id);
            return Ok(ResponseFactory.Ok());
        }
    }
}
