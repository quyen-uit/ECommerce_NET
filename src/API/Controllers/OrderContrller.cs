using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    public class OrderContrller : ApiControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderContrller(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPrinciple();
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethod, orderDto.BasketId, address);
            if (order == null)
            {
                return BadRequest(new ApiResponse(400, "Creating order fail"));
            }
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrders()
        {
            var email = HttpContext.User.RetrieveEmailFromPrinciple();
            var orders = await _orderService.GetOrdersByEmailAsync(email);
            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrinciple();
            var order = await _orderService.GetOrderByIdAsync(id, email);
            if (order == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [HttpGet("delivery-method")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethod()
        {
            var delivery = await _orderService.GetDeliveryMethodsAsync();
            return Ok(delivery);
        }
    }
}
