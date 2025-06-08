using Core.Common;
using Core.Enums;

namespace Core.Entities
{
    public class Size : BaseEntity
    {
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public SizeType SizeType { get; set; }
    }
}
