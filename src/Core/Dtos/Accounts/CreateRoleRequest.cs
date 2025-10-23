using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Accounts
{
    public class CreateRoleRequest
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Description { get; set; }
        public List<Guid> PermissionIds { get; set; } = new();
    }

    public class UpdateRoleRequest : CreateRoleRequest
    {
        [Required]
        public required string Id { get; set; }
    }
}
