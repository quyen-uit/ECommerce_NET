using Core.Common;
using Core.Entities.Identity;

namespace Core.Entities
{
    public class Wishlist : AuditableEntity
    {
        public long ProductSkuId { get; set; }
        public string AppUserId { get; set; } = string.Empty;
        public ProductSku ProductSku { get; set; } = default!;
        public AppUser AppUser { get; set; } = default!;
    }
}
