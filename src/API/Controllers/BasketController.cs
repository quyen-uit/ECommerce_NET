using API.Commons.Response;
using API.Helpers;
using MapsterMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class BasketController : ApiControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ApiSuccessResponse<CustomerBasketDto>>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            var result = _mapper.Map<CustomerBasketDto>(basket) ?? new CustomerBasketDto { Id = id };
            return Ok(ResponseFactory.Ok(result));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ApiSuccessResponse<CustomerBasketDto>>> UpdateBasket(CustomerBasketDto basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);
            var result = _mapper.Map<CustomerBasketDto>(updatedBasket);
            return Ok(ResponseFactory.Ok(result));
        }

        [AllowAnonymous]
        [HttpDelete]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
            return Ok(ResponseFactory.Ok());
        }
    }
}
