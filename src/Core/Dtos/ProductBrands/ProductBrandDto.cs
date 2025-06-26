using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ProductBrands
{
    public class ProductBrandDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }
}
