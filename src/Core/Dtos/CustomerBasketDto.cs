namespace Core.Dtos
{
    public class CustomerBasketDto
    {
        public string Id { get; set; } = string.Empty;
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public Guid? DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
