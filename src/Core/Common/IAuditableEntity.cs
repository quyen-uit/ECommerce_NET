
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Common
{
    public interface IAuditableEntity
    {
        public DateTime CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
    }
}
