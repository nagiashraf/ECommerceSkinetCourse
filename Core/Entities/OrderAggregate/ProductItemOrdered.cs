namespace Core.Entities.OrderAggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(int productItemId, string productName, string photoUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            PhotoUrl = photoUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}