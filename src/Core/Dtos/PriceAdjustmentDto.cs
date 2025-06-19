using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class PriceAdjustmentDto
    {
        public required string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<PriceAdjustmentItemDto> PriceAdjustmentItems { get; set; } = new List<PriceAdjustmentItemDto>();
    }

    public class PriceAdjustmentItemDto
    {
        public ProductDto? Product { get; set; }
        public decimal SalePrice { get; set; }
    }

    public class CreatePriceAdjustmentDto
    {
        [Required]
        public long ProductSkuId { get; set; }
        [Required]
        public decimal SalePrice { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public ICollection<CreatePriceAdjustmentItemDto> PriceAdjustmentItems { get; set; } = new List<CreatePriceAdjustmentItemDto>();
    }

    public class UpdatePriceAdjustmentDto : CreatePriceAdjustmentDto
    {
        [Required]
        public long Id { get; set; }
    }

    public class CreatePriceAdjustmentItemDto
    {
        public long? Id { get; set; }
        [Required]
        public long ProductId { get; set; }
        [Required]
        public decimal SalePrice { get; set; }
    }
}
