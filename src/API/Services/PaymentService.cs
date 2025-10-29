using Core.Constants;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Enums;
using Ardalis.Specification;
using Core.Interfaces.Services;
using Core.Interfaces.Reposiories;
using Core.Specifications.Orders;
using Stripe;
using Product = Core.Entities.Product;

namespace API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepositoryBase<Order> _orderRepository;
        private readonly IRepositoryBase<DeliveryMethod> _deliveryRepository;
        private readonly IRepositoryBase<Product> _productRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _config;

        public PaymentService(IRepositoryBase<Order> orderRepository, IRepositoryBase<DeliveryMethod> deliveryRepository, IRepositoryBase<Product> productRepository, IBasketRepository basketRepository, IConfiguration config)
        {
            _orderRepository = orderRepository;
            _deliveryRepository = deliveryRepository;
            _productRepository = productRepository;
            _basketRepository = basketRepository;
            _config = config;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null)
                throw new NotFoundException(CommonMessage.NotFoundBasket);

            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _deliveryRepository.GetByIdAsync(basket.DeliveryMethodId.Value);
                if (deliveryMethod == null)
                    throw new NotFoundException(CommonMessage.NotFoundDeliveryMethod);

                shippingPrice = deliveryMethod.Price;
            }

            // check price from db
            foreach (var item in basket.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.Id);
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
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await _orderRepository.FirstOrDefaultAsync(spec);

            if (order == null)
                throw new NotFoundException(CommonMessage.NotFoundOrder);

            order.Status = OrderStatus.PaymentFailed;
            await _orderRepository.UpdateAsync(order);
            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await _orderRepository.FirstOrDefaultAsync(spec);

            if (order == null)
                throw new NotFoundException(CommonMessage.NotFoundOrder);

            order.Status = OrderStatus.PaymentReceived;
            await _orderRepository.UpdateAsync(order);
            return order;
        }
    }
}
