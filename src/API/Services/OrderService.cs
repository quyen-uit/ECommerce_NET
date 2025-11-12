using Core.Exceptions;
using Core.Constants;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Interfaces.Reposiories;
using Core.Specifications.Orders;
using Core.Specifications.Products;

namespace API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepository)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, Guid deliveryId, string basketId, Address shipAddress)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var deliveryRepo = _unitOfWork.Repository<DeliveryMethod>();
            var productRepo = _unitOfWork.Repository<Product>();

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
            var products = await productRepo.ListAsync(new ProductsByIdsSpecification(productIds));
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
            var deliveryMethod = await deliveryRepo.GetByIdAsync(deliveryId);
            if (deliveryMethod == null)
                throw new NotFoundException(CommonMessage.NotFoundDeliveryMethod);

            decimal subTotal = items.Sum(i => i.Price * i.Quantity);

            // check order exist
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId!);
            var order = await orderRepo.FirstOrDefaultAsync(spec);

            if (order != null)
            {
                order.ShipToAddress = shipAddress;
                order.DeliveryMethod = deliveryMethod;
                order.Subtotal = subTotal;
                await orderRepo.UpdateAsync(order);
            }
            else
            {
                //create order
                order = new Order(items, buyerEmail, shipAddress, subTotal, deliveryMethod, basket.PaymentIntentId!);
                await orderRepo.AddAsync(order);
            }

            // Save all changes in one transaction (EF Core handles this automatically)
            await _unitOfWork.SaveChangesAsync();

            //await _basketRepository.DeleteBasketAsync(basketId);
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryRepo = _unitOfWork.Repository<DeliveryMethod>();
            return await deliveryRepo.ListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id, string email)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrdersWithItemsAndOrderingSpecification(id, email);
            var order = await orderRepo.FirstOrDefaultAsync(spec);
            if (order == null)
                throw new NotFoundException(CommonMessage.NotFoundOrder);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByEmailAsync(string email)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrdersWithItemsAndOrderingSpecification(email);
            var orders = await orderRepo.ListAsync(spec);
            return orders;
        }
    }
}
