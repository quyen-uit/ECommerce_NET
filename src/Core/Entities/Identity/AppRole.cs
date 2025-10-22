using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class AppRole : IdentityRole
    {
        public string Description { get; set; } = string.Empty;
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
