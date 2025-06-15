using Core.Common;
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
        [Range(0.1, double.MaxValue, ErrorMessage = MessageErrors.PriceMustBeGreaterThan)]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = MessageErrors.PriceMustBeGreaterThan)]
        public int Quantity { get; set; }
        [Required]
        public required string PhotoUrl { get; set; }
        [Required]
        public required string Brand { get; set; }
        [Required]
        public required string Type { get; set; }
    }
}
