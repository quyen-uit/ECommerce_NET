using API.Errors;
using Core.Common;
using Core.Constants;
using Core.Dtos;
using Core.Dtos.ProductSkus;
using Core.Interfaces.Services;
using Core.Specifications.ProductSkus;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductSkuController : Controller
    {
        public class ProductSkusController : ApiControllerBase
        {
            private readonly IProductSkuService _productSkuService;

            public ProductSkusController(IProductSkuService productSkuService)
            {
                _productSkuService = productSkuService;
            }

            [HttpPost("get-all")]
            public async Task<ActionResult<Pagination<ProductSkuDto>>> GetProductSkusFilterByName([FromBody] ProductSkuSpecParams productSkuSpecParams)
            {
                var result = await _productSkuService.GetAllProductSkusAsync(productSkuSpecParams);
                return Ok(result);
            }

            //[Cached(600)]
            [HttpGet("{id}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<ActionResult<ProductSkuDto>> GetProductSku(int id)
            {
                var productSku = await _productSkuService.GetProductSkuByIdAsync(id);
                return Ok(productSku);
            }

            [HttpPost("create")]
            public async Task<ActionResult<ProductSkuDto>> PostProductSku([FromBody]CreateProductSkuDto productSkuDto)
            {
                var result = await _productSkuService.AddOrUpdateProductSkuAsync(productSkuDto);
                if (result == null)
                    return BadRequest(new ApiResponse(400, CommonMessage.CreateFail));

                return Ok(result);
            }

            [HttpPost("update")]
            public async Task<IActionResult> PutProductSku([FromBody]CreateProductSkuDto productSkuDto)
            {
                var result = await _productSkuService.AddOrUpdateProductSkuAsync(productSkuDto);
                return Ok(result);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteProductSku(int id)
            {
                await _productSkuService.DeleteProductSkuAsync(id);
                return Ok(CommonMessage.DeleteSuccess);
            }
        }

    }
}
