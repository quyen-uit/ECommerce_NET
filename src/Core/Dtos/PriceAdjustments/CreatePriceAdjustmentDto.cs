using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.PriceAdjustments
{
    public class CreatePriceAdjustmentDto
    {
        public Guid? Id { get; set; }
        [Required]
        public Guid ProductSkuId { get; set; }
        [Required]
        public decimal SalePrice { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public ICollection<CreatePriceAdjustmentItemDto> PriceAdjustmentItems { get; set; } = new List<CreatePriceAdjustmentItemDto>();
    }
}
