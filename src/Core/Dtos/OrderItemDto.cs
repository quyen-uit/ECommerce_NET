
namespace Core.Dtos
{
    public class OrderItemDto
    {
        public Guid ProductSkuId { get; set; }
        public required string ProductName { get; set; }
        public string? PhotoUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
