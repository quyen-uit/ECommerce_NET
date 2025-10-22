namespace Core.Dtos.Accounts
{
    public class RoleResponse
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public List<PermissionResponse> Permissions { get; set; } = new();
        public int UserCount { get; set; }
    }
}
