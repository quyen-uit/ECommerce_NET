using Core.Entities;
using Core.Enums;

namespace Core.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PhotoUrl { get; set; }
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
        public int Stock { get; set; }
        public List<ProductSize> Size { get; set; } = new List<ProductSize>();
        public bool IsTrending { get; set; }
        public bool IsNew { get; set; }
    }
}
