using Core.Common;
using Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public required string ProductName { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = CommonMessage.PriceMustBeGreaterThan)]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = CommonMessage.PriceMustBeGreaterThan)]
        public int Quantity { get; set; }
        [Required]
        public required string PhotoUrl { get; set; }
        [Required]
        public required string Brand { get; set; }
        [Required]
        public required string Type { get; set; }
    }
}
