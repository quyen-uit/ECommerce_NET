using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Categories
{
    public class CategoryDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public long? ParentId { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}
