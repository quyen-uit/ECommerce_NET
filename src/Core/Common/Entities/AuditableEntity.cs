namespace Core.Common.Entities
{
    public abstract class AuditableEntity : BaseEntity, IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
