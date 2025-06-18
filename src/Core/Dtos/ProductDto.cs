using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Dtos
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

    public class CreateProductDto
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Description { get; set; }

        public string? PhotoUrl { get; set; }
        public bool IsTrending { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; } = true;
        public GenderType GenderType { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int ProductBrandId { get; set; }
        public ICollection<ProductPropertyDto> Properties { get; set; } = [];
    }

    public class UpdateProductDto : CreateProductDto
    {
        public long Id { get; set; }
    }

    public class ProductPropertyDto
    {
        [Required]
        [MaxLength(50)]
        public required string Key { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Value { get; set; }
    }
}
