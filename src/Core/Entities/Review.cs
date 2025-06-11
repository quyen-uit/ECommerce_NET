using Core.Common;
using Core.Entities.Identity;

namespace Core.Entities
{
    public class Review : AuditableEntity
    {
        public long ProductId { get; set; }
        public Product Product { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
    }

}
