using Core.Common;

namespace Core.Entities
{
    public class Size : BaseEntity
    {
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public long CategoryId { get; set; }
        public ICollection<Category> Category { get; set; }
    }
}
