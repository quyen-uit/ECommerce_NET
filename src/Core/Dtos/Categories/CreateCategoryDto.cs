namespace Core.Dtos.Categories
{
    public class CreateCategoryDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public int Order { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
