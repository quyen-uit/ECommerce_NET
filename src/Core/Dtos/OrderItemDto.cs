
namespace Core.Dtos
{
    public class OrderItemDto
    {
        public int ProductSkuId { get; set; }
        public string ProductName { get; set; }
        public string PhotoUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}