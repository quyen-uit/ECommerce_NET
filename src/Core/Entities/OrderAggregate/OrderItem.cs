using Core.Common.Entities;


namespace Core.Entities.OrderAggregate
{
    public class OrderItem : AuditableEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(OrderedProductItem item, decimal price, int quantity)
        {
            Item = item;
            Price = price;
            Quantity = quantity;
        }

        public OrderedProductItem Item { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
