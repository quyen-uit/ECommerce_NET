using API.Errors;
using Core.Common;
using Core.Constants;
using Core.Dtos.Sizes;
using Core.Interfaces.Services;
using Core.Specifications.Sizes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SizeController : ApiControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SizeDto>> GetSize(long id)
        {
            var dto = await _sizeService.GetSizeByIdAsync(id);
            return Ok(dto);
        }

        [HttpPost("get-all")]
        public async Task<ActionResult<Pagination<SizeDto>>> GetSizes(
            [FromBody] SizeSpecParams specParams
        )
        {
            var result = await _sizeService.GetAllSizesAsync(specParams);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<SizeDto>> CreateSize([FromBody] CreateSizeDto dto)
        {
            var result = await _sizeService.AddOrUpdateSizeAsync(dto);
            if (result == null)
                return BadRequest(new ApiResponse(400, CommonMessage.CreateFail));

            return Ok(result);
        }

        [HttpPost("create-many")]
        public async Task<ActionResult<IReadOnlyList<SizeDto>>> CreateSizes(
            [FromBody] IReadOnlyList<CreateSizeDto> dtos
        )
        {
            var result = await _sizeService.AddRangeSizeAsync(dtos);
            if (result == null || !result.Any())
                return BadRequest(new ApiResponse(400, CommonMessage.CreateFail));
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<ActionResult<SizeDto>> UpdateSize([FromBody] CreateSizeDto dto)
        {
            var result = await _sizeService.AddOrUpdateSizeAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteSize(long id)
        {
            await _sizeService.DeleteSizeAsync(id);
            return Ok(CommonMessage.DeleteSuccess);
        }

        [HttpDelete("delete-many")]
        public async Task<ActionResult<ApiResponse>> DeleteSizes([FromBody] List<long> ids)
        {
            await _sizeService.DeleteSizesAsync(ids);
            return Ok(CommonMessage.DeleteSuccess);
        }
    }
}
