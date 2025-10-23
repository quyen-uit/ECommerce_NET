using API.Commons.Response;
using API.Helpers;
using Core.Common;
using Core.Dtos.Sizes;
using Core.Interfaces.Services;
using Core.Specifications.Sizes;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "Permission:Size.Read")]
        public async Task<ActionResult<ApiSuccessResponse<SizeDto>>> GetSize(Guid id)
        {
            var dto = await _sizeService.GetSizeByIdAsync(id);
            return Ok(ResponseFactory.Ok(dto));
        }

        [HttpPost("search")]
        [Authorize(Policy = "Permission:Size.Read")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<SizeDto>>>> GetSizes(
                    [FromBody] SizeSpecParams specParams
                )
        {
            var result = await _sizeService.GetAllSizesAsync(specParams);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("create")]
        [Authorize(Policy = "Permission:Size.Create")]
        public async Task<ActionResult<ApiSuccessResponse<SizeDto>>> CreateSize([FromBody] CreateSizeDto dto)
        {
            var result = await _sizeService.AddOrUpdateSizeAsync(dto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("create-many")]
        [Authorize(Policy = "Permission:Size.Create")]
        public async Task<ActionResult<ApiSuccessResponse<IReadOnlyList<SizeDto>>>> CreateSizes(
            [FromBody] IReadOnlyList<CreateSizeDto> dtos
        )
        {
            var result = await _sizeService.AddRangeSizeAsync(dtos);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("update")]
        [Authorize(Policy = "Permission:Size.Update")]
        public async Task<ActionResult<ApiSuccessResponse<SizeDto>>> UpdateSize([FromBody] CreateSizeDto dto)
        {
            var result = await _sizeService.AddOrUpdateSizeAsync(dto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Permission:Size.Delete")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteSize(Guid id)
        {
            await _sizeService.DeleteSizeAsync(id);
            return Ok(ResponseFactory.Ok());
        }
        [HttpDelete("delete-many")]
        [Authorize(Policy = "Permission:Size.Delete")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteSizes([FromBody] List<Guid> ids)
        {
            await _sizeService.DeleteSizesAsync(ids);
            return Ok(ResponseFactory.Ok());
        }
    }
}
