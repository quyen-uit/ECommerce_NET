using Core.Common.Entities;
using Core.Entities.Identity;

namespace Core.Entities
{
    public class Review : AuditableEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public string AppUserId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; } = default!;

        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

}
