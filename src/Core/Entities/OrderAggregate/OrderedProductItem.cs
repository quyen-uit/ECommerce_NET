

namespace Core.Entities.OrderAggregate
{
    public class OrderedProductItem
    {
        public OrderedProductItem()
        {
        }

        public OrderedProductItem(Guid productSkuId, string productName, string photoUrl)
        {
            ProductSkuId = productSkuId;
            ProductName = productName;
            PhotoUrl = photoUrl;
        }

        public Guid ProductSkuId { get; set; }
        public string? ProductName { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
