using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Dtos.Sizes
{
    public class CreateSizeDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        public int SortOrder { get; set; }
        public SizeType SizeType { get; set; }
    }
}
