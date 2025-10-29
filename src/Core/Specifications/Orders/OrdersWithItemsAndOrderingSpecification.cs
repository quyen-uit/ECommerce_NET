using Ardalis.Specification;
using Core.Entities.OrderAggregate;


namespace Core.Specifications.Orders
{
    public class OrdersWithItemsAndOrderingSpecification : Specification<Order>
    {
        public OrdersWithItemsAndOrderingSpecification(string email)
        {
            Query.Where(o => o.BuyerEmail == email)
                 .Include(o => o.ShipToAddress)
                 .Include(o => o.DeliveryMethod)
                 .OrderByDescending(o => o.OrderDate);
        }
        public OrdersWithItemsAndOrderingSpecification(Guid id, string email)
        {
            Query.Where(o => o.Id == id && o.BuyerEmail == email)
                 .Include(o => o.ShipToAddress)
                 .Include(o => o.DeliveryMethod)
                 .Include(o => o.OrderItems);
        }
    }
}
