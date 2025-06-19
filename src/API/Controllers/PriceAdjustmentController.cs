using API.Errors;
using API.Helpers;
using Core.Common;
using Core.Constants;
using Core.Dtos;
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
        public async Task<ActionResult<PriceAdjustmentDto>> GetPriceAdjustment(long id)
        {
            var priceAdjustmentDto = await _priceAdjustmentService.GetPriceAdjustmentByIdAsync(id);
            return Ok(priceAdjustmentDto);
        }

        [HttpPost("get-all")]
        public async Task<ActionResult<Pagination<PriceAdjustmentDto>>> GetPriceAdjustments(
            [FromBody] PriceAdjustmentSpecParams specParams
        )
        {
            var result = await _priceAdjustmentService.GetAllPriceAdjustmentAsync(specParams);

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<PriceAdjustmentDto>> CreatePriceAdjustment(
            [FromBody] CreatePriceAdjustmentDto priceAdjustmentDto
        )
        {
            var result = await _priceAdjustmentService.AddPriceAdjustmentAsync(priceAdjustmentDto);
            if (result == null)
                return BadRequest(new ApiResponse(400, CommonMessage.CreateFail));

            return Ok(result);
        }


        [HttpPost("update")]
        public async Task<ActionResult<PriceAdjustmentDto>> UpdatePriceAdjustment(
            [FromBody] UpdatePriceAdjustmentDto priceAdjustmentDto
        )
        {
            var result = await _priceAdjustmentService.UpdatePriceAdjustmentAsync(priceAdjustmentDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeletePriceAdjustment(long id)
        {
            await _priceAdjustmentService.DeletePriceAdjustmentAsync(id);
            return Ok(CommonMessage.DeleteSuccess);
        }
    }
}
