using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Categories
{

    public class CreateCategoryDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public long? ParentId { get; set; }
        public int Order { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }

}
