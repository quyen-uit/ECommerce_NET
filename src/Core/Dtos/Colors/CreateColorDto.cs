using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Colors
{

    public class CreateColorDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [MaxLength(20)]
        public required string Name { get; set; }

        [Required]
        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Hex code is not valid")]
        public required string HexCode { get; set; }
    }

}
