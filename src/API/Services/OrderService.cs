using Core.Exceptions;
using Core.Constants;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Interfaces.Services;
using Core.Interfaces.Reposiories;
using Core.Specifications.Orders;
using Core.Specifications.Products;

namespace API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<DeliveryMethod> _deliveryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IBasketRepository _basketRepository;

        public OrderService(IRepository<Order> orderRepository, IRepository<DeliveryMethod> deliveryRepository, IRepository<Product> productRepository, IBasketRepository basketRepository)
        {
            _orderRepository = orderRepository;
            _deliveryRepository = deliveryRepository;
            _productRepository = productRepository;
            _basketRepository = basketRepository;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, Guid deliveryId, string basketId, Address shipAddress)
        {
            // get basket   
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null)
            {
                throw new BadRequestException("Basket not found");
            }

            //get item from product
            var items = new List<OrderItem>();

            // Fix: Batch fetch all products in one query instead of N queries
            var productIds = basket!.Items.Select(i => i.Id).ToList();
            var products = await _productRepository.ListAsync(new ProductsByIdsSpecification(productIds));
            var productDict = products.ToDictionary(p => p.Id);

            foreach (var basketItem in basket.Items)
            {
                if (!productDict.TryGetValue(basketItem.Id, out var product))
                    throw new NotFoundException(CommonMessage.NotFoundProduct);

                OrderedProductItem orderProductItem = new OrderedProductItem(product.Id, product.Name, product.PhotoUrl!);
                OrderItem orderItem = new OrderItem(orderProductItem, basketItem.Price, basketItem.Quantity);

                items.Add(orderItem);
            }
            // get delivery method
            var deliveryMethod = await _deliveryRepository.GetByIdAsync(deliveryId);
            if (deliveryMethod == null)
                throw new NotFoundException(CommonMessage.NotFoundDeliveryMethod);

            decimal subTotal = items.Sum(i => i.Price * i.Quantity);

            // check order exist
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId!);
            var order = await _orderRepository.FirstOrDefaultAsync(spec);

            if (order != null)
            {
                order.ShipToAddress = shipAddress;
                order.DeliveryMethod = deliveryMethod;
                order.Subtotal = subTotal;
                await _orderRepository.UpdateAsync(order);
            }
            else
            {
                //create order
                order = new Order(items, buyerEmail, shipAddress, subTotal, deliveryMethod, basket.PaymentIntentId!);
                await _orderRepository.AddAsync(order);
            }

            //await _basketRepository.DeleteBasketAsync(basketId);
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _deliveryRepository.ListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id, string email)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, email);
            var order = await _orderRepository.FirstOrDefaultAsync(spec);
            if (order == null)
                throw new NotFoundException(CommonMessage.NotFoundOrder);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByEmailAsync(string email)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(email);
            var orders = await _orderRepository.ListAsync(spec);
            return orders;
        }
    }
}
