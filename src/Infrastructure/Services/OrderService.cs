using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
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

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryId, string basketId, Address shipAddress)
        {
            // get basket   
            var basket = await _basketRepository.GetBasketAsync(basketId);

            //get item from product
            var items = new List<OrderItem>();

            foreach (var basketItem in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(basketItem.Id);

                OrderedProductItem orderProductItem = new OrderedProductItem(product.Id, product.Name, product.PhotoUrl);
                OrderItem orderItem = new OrderItem(orderProductItem, basketItem.Price, basketItem.Quantity);

                items.Add(orderItem);
            }
            // get delivery method
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryId);

            decimal subTotal = items.Sum(i => i.Price * i.Quantity);

            // check order exist
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (order != null)
            {
                order.ShipToAddress = shipAddress;
                order.DeliveryMethod = deliveryMethod;
                order.Subtotal = subTotal;
                _unitOfWork.Repository<Order>().Update(order);
            } 
            else
            {
                //create order
                order = new Order(items, buyerEmail, shipAddress, subTotal, deliveryMethod, basket.PaymentIntentId);
                _unitOfWork.Repository<Order>().Add(order);
            }

            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                return null;
            }

            //await _basketRepository.DeleteBasketAsync(basketId);
            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string email)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, email);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByEmailAsync(string email)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(email);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
    }
}
