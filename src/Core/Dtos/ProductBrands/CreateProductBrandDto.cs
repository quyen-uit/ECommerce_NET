using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ProductBrands
{
    public class CreateProductBrandDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }

}
