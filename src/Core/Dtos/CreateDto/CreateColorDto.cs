using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CreateDto
{
    public class CreateColorDto
    {
        [Required]
        [MaxLength(20)]
        public required string Name { get; set; }
        [Required]
        [MaxLength(7)]
        [MinLength(7)]
        public required string HexCode { get; set; }

    }

}
