using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ProductSkus
{
    public class CreateProductSkuDto
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string SkuCode { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid ColorId { get; set; }

        [Required]
        public Guid SizeId { get; set; }

        public bool IsActive { get; set; } = true;
        // public ICollection<CreateOrUpdateImageDto> Images { get; set; } = new List<CreateOrUpdateImageDto>();

    }

}
