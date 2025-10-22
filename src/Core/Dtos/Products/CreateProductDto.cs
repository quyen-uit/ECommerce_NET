using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Products
{
    public class CreateProductDto
    {
        [Required]
        public long Id { get; set; }
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
        public ICollection<ProductPropertyDto> Properties { get; set; } = new List<ProductPropertyDto>();
    }
}
