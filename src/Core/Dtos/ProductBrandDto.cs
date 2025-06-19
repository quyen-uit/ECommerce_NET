using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class ProductBrandDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }

    public class CreateProductBrandDto
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }

    public class UpdateProductBrandDto : CreateProductBrandDto
    {
        [Required]
        public long Id { get; set; }
    }
}
