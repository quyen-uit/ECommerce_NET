using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class CategoryDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public long? ParentId { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public long? ParentId { get; set; }
        public int Order { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateCategoryDto : CreateCategoryDto
    {
        public long Id { get; set; }
    }
}
