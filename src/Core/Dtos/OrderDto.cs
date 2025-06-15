namespace Core.Dtos
{
    public class OrderDto
    {
        public required string BasketId { get; set; }
        public int DeliveryMethod { get; set; }
        public required AddressDto ShipToAddress { get; set; }
    }
}
