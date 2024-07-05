using Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = MessageErrors.PriceMustBeGreaterThan)]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = MessageErrors.PriceMustBeGreaterThan)]
        public int Quantity { get; set; }
        [Required]
        public string PhotoUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
