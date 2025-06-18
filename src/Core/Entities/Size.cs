using Core.Common.Entities;
using Core.Enums;

namespace Core.Entities
{
    public class Size : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public SizeType SizeType { get; set; }
    }
}
