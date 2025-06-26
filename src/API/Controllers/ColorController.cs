using API.Errors;
using Core.Common;
using Core.Constants;
using Core.Dtos;
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
        public async Task<ActionResult<ColorDto>> GetColor(long id)
        {
            var colorDto = await _colorService.GetColorByIdAsync(id);
            return Ok(colorDto);
        }

        // 🔄 POST: api/color/get-all
        [HttpPost("get-all")]
        public async Task<ActionResult<Pagination<ColorDto>>> GetColors(
            [FromBody] ColorSpecParams specParams
        )
        {
            var result = await _colorService.GetAllColorsAsync(specParams);
            return Ok(result);
        }

        // ✅ POST: api/color/create
        [HttpPost("create")]
        public async Task<ActionResult<ColorDto>> CreateColor([FromBody] CreateColorDto colorDto)
        {
            var result = await _colorService.AddOrUpdateColorAsync(colorDto);
            if (result == null)
                return BadRequest(new ApiResponse(400, CommonMessage.CreateFail));

            return Ok(result);
        }

        // ✅ POST: api/color/create-many
        [HttpPost("create-many")]
        public async Task<ActionResult<IReadOnlyList<ColorDto>>> CreateColors(
            [FromBody] IReadOnlyList<CreateColorDto> colorDtos
        )
        {
            var result = await _colorService.AddRangeColorAsync(colorDtos);
            if (result == null || !result.Any())
                return BadRequest(new ApiResponse(400, CommonMessage.CreateFail));

            return Ok(result);
        }

        // ✅ POST: api/color/update
        [HttpPost("update")]
        public async Task<ActionResult<ColorDto>> UpdateColor([FromBody] CreateColorDto colorDto)
        {
            var result = await _colorService.AddOrUpdateColorAsync(colorDto);
            return Ok(result);
        }

        // ✅ DELETE: api/color/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteColor(long id)
        {
            await _colorService.DeleteColorAsync(id);
            return Ok(CommonMessage.DeleteSuccess);
        }
    }
}
