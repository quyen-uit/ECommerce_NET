using System.ComponentModel.DataAnnotations;
using Core.Dtos.Categories;
using Core.Dtos.ProductBrands;
using Core.Enums;

namespace Core.Dtos.Products
{
    public class ProductDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsTrending { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public GenderType GenderType { get; set; }

        public CategoryDto? Category { get; set; }
        public ProductBrandDto? ProductBrand { get; set; }
        // public ICollection<Review> Reviews { get; set; } = new List<Review>();
        // public ICollection<ProductSku> ProductSkus { get; set; } = new List<ProductSku>();
        public ICollection<ProductPropertyDto> Properties { get; set; } = new List<ProductPropertyDto>();   
        // public ICollection<Collection> Collections { get; set; } = new List<Collection>();

    }
}
