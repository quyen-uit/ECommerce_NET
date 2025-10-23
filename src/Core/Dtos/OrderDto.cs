namespace Core.Dtos
{
    public class OrderDto
    {
        public required string BasketId { get; set; }
        public Guid DeliveryMethod { get; set; }
        public required AddressDto ShipToAddress { get; set; }
    }
}
