using System.ComponentModel.DataAnnotations;
using Core.Dtos.Products;

namespace Core.Dtos.PriceAdjustments
{
    public class PriceAdjustmentItemDto
    {
        public ProductDto? Product { get; set; }
        public decimal SalePrice { get; set; }
    }
}
