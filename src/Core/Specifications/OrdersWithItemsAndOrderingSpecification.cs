using Core.Common;
using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecification(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.ShipToAddress);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
        }
        public OrdersWithItemsAndOrderingSpecification(int id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.ShipToAddress);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}
