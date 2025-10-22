using API.Commons.Response;
using API.Helpers;
using Core.Common;
using Core.Dtos.PriceAdjustments;
using Core.Interfaces.Services;
using Core.Specifications.PriceAdjustments;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PriceAdjustmentController : ApiControllerBase
    {
        private readonly IPriceAdjustmentService _priceAdjustmentService;

        public PriceAdjustmentController(IPriceAdjustmentService priceAdjustmentService)
        {
            _priceAdjustmentService = priceAdjustmentService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<PriceAdjustmentDto>>> GetPriceAdjustment(long id)
        {
            var priceAdjustmentDto = await _priceAdjustmentService.GetPriceAdjustmentByIdAsync(id);
            return Ok(ResponseFactory.Ok(priceAdjustmentDto));
        }

        [HttpPost("get-all")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<PriceAdjustmentDto>>>> GetPriceAdjustments(
            [FromBody] PriceAdjustmentSpecParams specParams
        )
        {
            var result = await _priceAdjustmentService.GetAllPriceAdjustmentAsync(specParams);

            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiSuccessResponse<PriceAdjustmentDto>>> CreatePriceAdjustment(
            [FromBody] CreatePriceAdjustmentDto priceAdjustmentDto
        )
        {
            var result = await _priceAdjustmentService.AddOrUpdatePriceAdjustmentAsync(priceAdjustmentDto);
            return Ok(ResponseFactory.Ok(result));
        }


        [HttpPost("update")]
        public async Task<ActionResult<ApiSuccessResponse<PriceAdjustmentDto>>> UpdatePriceAdjustment(
            [FromBody] CreatePriceAdjustmentDto priceAdjustmentDto
        )
        {
            var result = await _priceAdjustmentService.AddOrUpdatePriceAdjustmentAsync(priceAdjustmentDto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeletePriceAdjustment(long id)
        {
            await _priceAdjustmentService.DeletePriceAdjustmentAsync(id);
            return Ok(ResponseFactory.Ok());
        }
    }
}
