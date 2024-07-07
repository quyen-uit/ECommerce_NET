using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CreateDto
{
    public class CreateProductColorDto
    {
        [Required]
        public int ColorId { get; set; }
        [Required] 
        public List<string> PhotoUrls { get; set; }

    }

}
