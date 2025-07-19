using API.Commons;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpGet]
        public async Task<ActionResult<ApiSuccessResponse<IReadOnlyList<OrderToReturnDto>>>> GetOrders()
        {
            var email = HttpContext.User.RetrieveEmailFromPrinciple();
            var orders = await _orderService.GetOrdersByEmailAsync(email!);
            var result = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<OrderToReturnDto>>> GetOrderById(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrinciple();
            var order = await _orderService.GetOrderByIdAsync(id, email!);
            var result = _mapper.Map<OrderToReturnDto>(order);
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
