namespace API.DTOs
{
    public class OrderDto
    {
        public string basketId { get; set; } = string.Empty;
        public int DeliveryMethodId { get; set; }
        public AddressDto ShipToAddress { get; set; } = new();
    }
}