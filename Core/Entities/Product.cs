namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PhotoUrl { get; set; }
        public ProductType ProductType { get; set; } = null!;
        public int ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; } = null!;
        public int ProductBrandId { get; set; }

        public Product(string name, string description, string photoUrl) 
        {
            Name = name;
            Description = description;
            PhotoUrl = photoUrl;
        }
    }
}