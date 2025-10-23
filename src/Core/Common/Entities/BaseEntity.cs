using System.ComponentModel.DataAnnotations;

namespace Core.Common.Entities
{
    public abstract class BaseEntity : ISoftDeletable
    {

        [Key]
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}

