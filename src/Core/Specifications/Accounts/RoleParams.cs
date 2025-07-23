using Core.Common.Specifications;

namespace Core.Specifications.Accounts
{
    public class RoleParams : QueryStringParameter
    {
        public RoleFilter Filter { get; set; } = new RoleFilter();
    }
    public class RoleFilter
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
