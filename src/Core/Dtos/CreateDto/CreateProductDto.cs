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

        public string PhotoUrl { get; set; }
        public List<ProductSize> Size { get; set; }
        public bool IsTrending { get; set; }
        public bool IsNew { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int ProductBrandId { get; set; }
        //public ICollection<CreateProductColorDto> ProductColors { get; set; }
    }
}
