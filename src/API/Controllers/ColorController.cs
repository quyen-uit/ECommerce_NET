using API.Commons.Response;
using API.Helpers;
using Core.Common;
using Core.Dtos.Colors;
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
        public async Task<ActionResult<ApiSuccessResponse<ColorDto>>> GetColor(long id)
        {
            var colorDto = await _colorService.GetColorByIdAsync(id);
            return Ok(ResponseFactory.Ok(colorDto));
        }

        // 🔄 POST: api/color/get-all
        [HttpPost("get-all")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<ColorDto>>>> GetColors(
            [FromBody] ColorSpecParams specParams
        )
        {
            var result = await _colorService.GetAllColorsAsync(specParams);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/color/create
        [HttpPost("create")]
        public async Task<ActionResult<ApiSuccessResponse<ColorDto>>> CreateColor([FromBody] CreateColorDto colorDto)
        {
            var result = await _colorService.AddOrUpdateColorAsync(colorDto);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/color/create-many
        [HttpPost("create-many")]
        public async Task<ActionResult<ApiSuccessResponse<IReadOnlyList<ColorDto>>>> CreateColors(
            [FromBody] IReadOnlyList<CreateColorDto> colorDtos
        )
        {
            var result = await _colorService.AddRangeColorAsync(colorDtos);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/color/update
        [HttpPost("update")]
        public async Task<ActionResult<ApiSuccessResponse<ColorDto>>> UpdateColor([FromBody] CreateColorDto colorDto)
        {
            var result = await _colorService.AddOrUpdateColorAsync(colorDto);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ DELETE: api/color/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteColor(long id)
        {
            await _colorService.DeleteColorAsync(id);
            return Ok(ResponseFactory.Ok());
        }
    }
}
