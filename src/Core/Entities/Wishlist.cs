using Core.Common;
using Core.Entities.Identity;

namespace Core.Entities
{
    public class Wishlist : BaseEntity
    {
        public long ProductSkuId { get; set; }
        public string AppUserId { get; set; }
        public ProductSku ProductSku { get; set; }
        public AppUser AppUser { get; set; }
    }
}
