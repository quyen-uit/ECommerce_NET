using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CreateDto
{
    public class CreateProductBrandDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
