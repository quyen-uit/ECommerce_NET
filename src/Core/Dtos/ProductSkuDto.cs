using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Dtos
{
    public class ProductSkuDto
    {
        public long Id { get; set; }
        public required string SkuCode { get; set; }
        public bool IsActive { get; set; } = true;
        public ColorDto Color { get; set; } = default!;
        public SizeDto Size { get; set; } = default!;
        public ICollection<ImageDto> Images { get; set; } = new List<ImageDto>();
    }

    public class CreateProductSkuDto
    {
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

    public class UpdateProductSkuDto : CreateProductSkuDto
    {
        public long Id { get; set; }
    }

}
