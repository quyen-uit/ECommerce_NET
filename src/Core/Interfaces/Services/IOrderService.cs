using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, Guid deliveryMethod, string basketId, Address shipAddress);
        Task<IReadOnlyList<Order>> GetOrdersByEmailAsync(string email);
        Task<Order> GetOrderByIdAsync(Guid id, string email);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
