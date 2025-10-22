namespace Core.Entities.Identity
{
    public class RolePermission
    {
        public string RoleId { get; set; } = string.Empty;
        public long PermissionId { get; set; }

        public AppRole Role { get; set; } = default!;
        public Permission Permission { get; set; } = default!;
    }
}
