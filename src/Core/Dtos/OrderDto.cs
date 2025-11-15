namespace Core.Dtos
{
    public class OrderDto
    {
        public string BasketId { get; set; } = string.Empty;
        public Guid DeliveryMethod { get; set; }
        public AddressDto ShipToAddress { get; set; } = new();
    }
}
