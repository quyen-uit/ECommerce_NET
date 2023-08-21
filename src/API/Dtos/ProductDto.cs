using Core.Entities;

namespace API.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PhotoUrl { get; set; }
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
    }
}
