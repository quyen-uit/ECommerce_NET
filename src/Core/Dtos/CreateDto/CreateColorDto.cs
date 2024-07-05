using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CreateDto
{
    public class CreateColorDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(7)]
        [MinLength(7)]
        public string HexCode { get; set; }

    }

}
