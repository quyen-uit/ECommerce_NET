using API.Commons.Response;
using API.Helpers;
using Core.Common;
using Core.Constants;
using Core.Dtos.ProductSkus;
using Core.Interfaces.Services;
using Core.Specifications.ProductSkus;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductSkuController : ApiControllerBase
    {
        private readonly IProductSkuService _productSkuService;

        public ProductSkuController(IProductSkuService productSkuService)
        {
            _productSkuService = productSkuService;
        }

        [HttpPost("get-all")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<ProductSkuDto>>>> GetProductSkusFilterByName([FromBody] ProductSkuSpecParams productSkuSpecParams)
        {
            var result = await _productSkuService.GetAllProductSkusAsync(productSkuSpecParams);
            return Ok(ResponseFactory.Ok(result));
        }

        //[Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiSuccessResponse<ProductSkuDto>>> GetProductSku(int id)
        {
            var productSku = await _productSkuService.GetProductSkuByIdAsync(id);
            return Ok(ResponseFactory.Ok(productSku));
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiSuccessResponse<ProductSkuDto>>> PostProductSku([FromBody]CreateProductSkuDto productSkuDto)
        {
            var result = await _productSkuService.AddOrUpdateProductSkuAsync(productSkuDto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("update")]
        public async Task<ActionResult<ApiSuccessResponse<ProductSkuDto>>> PutProductSku([FromBody]CreateProductSkuDto productSkuDto)
        {
            var result = await _productSkuService.AddOrUpdateProductSkuAsync(productSkuDto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteProductSku(int id)
        {
            await _productSkuService.DeleteProductSkuAsync(id);
            return Ok(ResponseFactory.Ok());
        }
    }
}
