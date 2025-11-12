using Core.Constants;
using Core.Exceptions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Enums;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Interfaces.Reposiories;
using Core.Specifications.Orders;
using Stripe;
using Product = Core.Entities.Product;

namespace API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _config;

        public PaymentService(IUnitOfWork unitOfWork, IBasketRepository basketRepository, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _config = config;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var deliveryRepo = _unitOfWork.Repository<DeliveryMethod>();
            var productRepo = _unitOfWork.Repository<Product>();

            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null)
                throw new NotFoundException(CommonMessage.NotFoundBasket);

            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await deliveryRepo.GetByIdAsync(basket.DeliveryMethodId.Value);
                if (deliveryMethod == null)
                    throw new NotFoundException(CommonMessage.NotFoundDeliveryMethod);

                shippingPrice = deliveryMethod.Price;
            }

            // check price from db
            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id);
                if (product == null)
                    throw new NotFoundException(CommonMessage.NotFoundProduct);

                if (product.Price != item.Price)
                {
                    item.Price = product.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var option = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * i.Price * 100) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(option);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var option = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * i.Price * 100) + (long)shippingPrice * 100,
                };
                await service.UpdateAsync(basket.PaymentIntentId, option);
            }

            await _basketRepository.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await orderRepo.FirstOrDefaultAsync(spec);

            if (order == null)
                throw new NotFoundException(CommonMessage.NotFoundOrder);

            order.Status = OrderStatus.PaymentFailed;
            await orderRepo.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await orderRepo.FirstOrDefaultAsync(spec);

            if (order == null)
                throw new NotFoundException(CommonMessage.NotFoundOrder);

            order.Status = OrderStatus.PaymentReceived;
            await orderRepo.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return order;
        }
    }
}
