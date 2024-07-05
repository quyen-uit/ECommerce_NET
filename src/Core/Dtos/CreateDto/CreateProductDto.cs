using Core.Entities;
using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CreateDto
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        [MinLength(0)]
        public decimal Price { get; set; }
        public string PhotoUrl { get; set; }
        public int Stock { get; set; }
        public ProductSize Size { get; set; }
        public bool IsTrending { get; set; }
        public bool IsNew { get; set; }
        public int CategoryId { get; set; }
        public int ProductBrandId { get; set; }
        public int ColorId { get; set; }
        public ICollection<string> PictureUrls { get; set; }
    }
}
