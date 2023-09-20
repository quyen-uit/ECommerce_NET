using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
