using Core.Common.Entities;
using Core.Enums;

namespace Core.Entities
{

    public class Image : AuditableEntity
    {
        public string Url { get; set; } = string.Empty;

        public Guid ReferenceId { get; set; }
        public int Order { get; set; } = 0;

        public ImageType Type { get; set; } // Enum: Sku, Brand, Review, etc.
    }

}
