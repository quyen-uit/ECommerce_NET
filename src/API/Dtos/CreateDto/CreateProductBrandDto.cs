using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CreateProductBrandDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
