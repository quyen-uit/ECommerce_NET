using Core.Common.Entities;
using Core.Entities.Identity;
using Core.Enums;

namespace Core.Entities.OrderAggregate
{
    public class Order : AuditableEntity
    {
        public Order()
        {
        }

        public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, Address shipToAddress, decimal subtotal, DeliveryMethod deliveryMethod, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            Subtotal = subtotal;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            PaymentIntentId = paymentIntentId;
        }

        public string? BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public long ShipToAddressId { get; set; }
        public Address ShipToAddress { get; set; } = default!;
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? PaymentIntentId { get; set; }
        public PaymentType PaymentType { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public IReadOnlyList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }
}
