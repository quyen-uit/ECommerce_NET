namespace Core.Dtos.Categories
{
    public class CategoryNodeDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public List<CategoryNodeDto> ChildCategories { get; set; } = [];
    }
}
