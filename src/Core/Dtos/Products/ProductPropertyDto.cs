using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Products
{
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
