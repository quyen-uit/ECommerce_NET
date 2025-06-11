using Core.Common;
using Core.Enums;

namespace Core.Entities
{

    public class Image : AuditableEntity
    {
        public string Url { get; set; }

        public long ReferenceId { get; set; }

        public ImageType Type { get; set; } // Enum: Sku, Brand, Review, etc.
    }

}
