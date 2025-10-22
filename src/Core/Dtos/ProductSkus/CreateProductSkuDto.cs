using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ProductSkus
{
    public class CreateProductSkuDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string SkuCode { get; set; }

        [Required]
        public long ProductId { get; set; }

        [Required]
        public long ColorId { get; set; }

        [Required]
        public long SizeId { get; set; }

        public bool IsActive { get; set; } = true;
        // public ICollection<CreateOrUpdateImageDto> Images { get; set; } = new List<CreateOrUpdateImageDto>();

    }

}
