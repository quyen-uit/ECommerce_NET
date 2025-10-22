using API.Commons.Response;
using API.Extensions;
using API.Helpers;
using MapsterMapper;
using Core.Dtos;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [Authorize]
    public class OrderController : ApiControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ApiSuccessResponse<OrderToReturnDto>>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPrinciple();
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(email!, orderDto.DeliveryMethod, orderDto.BasketId, address);
            var result = _mapper.Map<OrderToReturnDto>(order);
            // Ensure item photo URLs are absolute using ApiUrl
            var apiUrl = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["ApiUrl"]; 
            if (!string.IsNullOrEmpty(apiUrl))
            {
                foreach (var item in result.OrderItems)
                {
                    if (!string.IsNullOrEmpty(item.PhotoUrl))
                    {
                        item.PhotoUrl = apiUrl + item.PhotoUrl;
                    }
                }
            }
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpGet]
        public async Task<ActionResult<ApiSuccessResponse<IReadOnlyList<OrderToReturnDto>>>> GetOrders()
        {
            var email = HttpContext.User.RetrieveEmailFromPrinciple();
            var orders = await _orderService.GetOrdersByEmailAsync(email!);
            var result = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
            var apiUrl = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["ApiUrl"]; 
            if (!string.IsNullOrEmpty(apiUrl))
            {
                foreach (var o in result)
                {
                    foreach (var item in o.OrderItems)
                    {
                        if (!string.IsNullOrEmpty(item.PhotoUrl))
                        {
                            item.PhotoUrl = apiUrl + item.PhotoUrl;
                        }
                    }
                }
            }
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<OrderToReturnDto>>> GetOrderById(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrinciple();
            var order = await _orderService.GetOrderByIdAsync(id, email!);
            var result = _mapper.Map<OrderToReturnDto>(order);
            var apiUrl = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["ApiUrl"]; 
            if (!string.IsNullOrEmpty(apiUrl))
            {
                foreach (var item in result.OrderItems)
                {
                    if (!string.IsNullOrEmpty(item.PhotoUrl))
                    {
                        item.PhotoUrl = apiUrl + item.PhotoUrl;
                    }
                }
            }
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<ApiSuccessResponse<IReadOnlyList<DeliveryMethod>>>> GetDeliveryMethods()
        {
            var delivery = await _orderService.GetDeliveryMethodsAsync();
            return Ok(ResponseFactory.Ok(delivery));
        }
    }
}
