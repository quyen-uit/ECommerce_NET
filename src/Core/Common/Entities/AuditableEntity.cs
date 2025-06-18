using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Common.Entities
{
    public abstract class AuditableEntity : BaseEntity, IAuditableEntity
    {
        public DateTime CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
    }
}
