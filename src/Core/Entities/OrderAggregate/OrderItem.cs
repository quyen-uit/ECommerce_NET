using Core.Common;


namespace Core.Entities.OrderAggregate
{
    public class OrderItem : BaseEntity
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

        public OrderedProductItem Item { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
