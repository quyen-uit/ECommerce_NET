using Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public int Order { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public Category Parent { get; set; }
        public ICollection<Category> SubCategories { get; set; }
    }
}