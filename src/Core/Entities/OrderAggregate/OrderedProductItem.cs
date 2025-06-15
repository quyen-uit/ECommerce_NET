

namespace Core.Entities.OrderAggregate
{
    public class OrderedProductItem
    {
        public OrderedProductItem()
        {
        }

        public OrderedProductItem(long productSkuId, string productName, string photoUrl)
        {
            ProductSkuId = productSkuId;
            ProductName = productName;
            PhotoUrl = photoUrl;
        }

        public long ProductSkuId { get; set; }
        public string? ProductName { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
