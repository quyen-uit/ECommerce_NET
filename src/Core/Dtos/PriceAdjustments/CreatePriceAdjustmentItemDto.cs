using System.ComponentModel.DataAnnotations;
using Core.Dtos.Products;

namespace Core.Dtos.PriceAdjustments
{    public class CreatePriceAdjustmentItemDto
    {
        [Required]
        public long? Id { get; set; }
        [Required]
        public long ProductId { get; set; }
        [Required]
        public decimal SalePrice { get; set; }
    }
}
