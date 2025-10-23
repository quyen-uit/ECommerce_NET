namespace Core.Dtos.Categories
{
    public class CategoryNodeDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public List<CategoryNodeDto> ChildCategories { get; set; } = [];
    }
}
