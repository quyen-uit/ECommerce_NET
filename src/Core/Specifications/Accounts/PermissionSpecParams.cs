using Core.Common.Specifications;

namespace Core.Specifications.Accounts
{
    public class PermissionSpecParams : QueryStringParameter
    {
        public PermissionFilter Filter { get; set; } = new PermissionFilter();
    }
    public class PermissionFilter
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Module { get; set; }
        public string? Action { get; set; }
    }
}