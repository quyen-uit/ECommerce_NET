using Core.Common;
using Core.Entities.Identity;

namespace Core.Entities
{
    public class Collection : AuditableEntity
    {

        public string Name { get; set; }
        public string Slug { get; set; } // SEO-friendly URL
        public string Description { get; set; }
        public string BannerImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Product> Products { get; set; }

    }

}
