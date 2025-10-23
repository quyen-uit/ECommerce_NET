using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Sizes
{
    public class CreateSizeDto
    {
        public Guid? Id { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        public int SortOrder { get; set; }
        [Required]
        public required string SizeType { get; set; }
    }
}
