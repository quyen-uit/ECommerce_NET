using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class OrderedProductItem
    {
        public OrderedProductItem()
        {
        }

        public OrderedProductItem(int productId, string productName, string photoUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PhotoUrl = photoUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
