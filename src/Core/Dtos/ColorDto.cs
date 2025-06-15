using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class ColorDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string HexCode { get; set; }
    }

    public class CreateColorDto
    {
        [Required]
        [MaxLength(20)]
        public required string Name { get; set; }

        [Required]
        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Hex code is not valid")]
        public required string HexCode { get; set; }
    }

    public class UpdateColorDto : CreateColorDto
    {
        public long Id { get; set; }
    }
}
