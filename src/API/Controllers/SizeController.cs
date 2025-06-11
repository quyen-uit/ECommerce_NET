using API.Errors;
using API.Helpers;
using Core.Dtos;
using Core.Interfaces.Services;
using Core.Specifications.Colors;
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
            if (dto == null)
                return NotFound(new ApiException(404, "Size not found"));

            return Ok(dto);
        }

        [HttpPost("get-all")]
        public async Task<ActionResult<Pagination<SizeDto>>> GetSizes(
            [FromBody] SizeSpecParams specParams
        )
        {
            var dtos = await _sizeService.GetAllSizesAsync(specParams);
            return Ok(
                new Pagination<SizeDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: dtos.Count,
                    data: dtos
                )
            );
        }

        [HttpPost("create")]
        public async Task<ActionResult<SizeDto>> CreateSize([FromBody] CreateSizeDto dto)
        {
            var result = await _sizeService.AddSizeAsync(dto);
            return CreatedAtAction(nameof(GetSize), new { id = result.Id }, result);
        }

        [HttpPost("create-many")]
        public async Task<ActionResult<IReadOnlyList<SizeDto>>> CreateSizes(
            [FromBody] IReadOnlyList<CreateSizeDto> dtos
        )
        {
            var result = await _sizeService.AddRangeSizeAsync(dtos);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<ActionResult<SizeDto>> UpdateSize([FromBody] UpdateSizeDto dto)
        {
            var result = await _sizeService.UpdateSizeAsync(dto);
            if (result == null)
                return NotFound(new ApiException(404, "Size not found"));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteSize(long id)
        {
            var success = await _sizeService.DeleteSizeAsync(id);
            if (!success)
                return NotFound(new ApiException(404, "Size not found"));

            return Ok(new ApiResponse(200, "Size deleted successfully"));
        }
    }
}
