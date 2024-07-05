using Core.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket != null)
            {
                var customerBasket = _mapper.Map<CustomerBasketDto>(basket);
                return customerBasket;
            }
            else
            {
                return new CustomerBasketDto { Id = id };
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }

    }
}
