namespace Core.Dtos.Admin
{
    public class SoftDeletedItemDto
    {
        public Guid Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public DateTime DeletedAt { get; set; }
    }
}
