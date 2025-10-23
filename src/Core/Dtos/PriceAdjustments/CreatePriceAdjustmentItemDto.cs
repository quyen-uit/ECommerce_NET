using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.PriceAdjustments
{
    public class CreatePriceAdjustmentItemDto
    {
        [Required]
        public Guid? Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public decimal SalePrice { get; set; }
    }
}
