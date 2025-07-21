using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Accounts
{
    public class CreatePermissionRequest
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Description { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Module { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Action { get; set; }
    }

    public class UpdatePermissionRequest : CreatePermissionRequest
    {
        [Required]
        public required int Id { get; set; }
    }
}