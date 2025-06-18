using Core.Common.Entities;

namespace Core.Entities.Inventory
{

    public class Vendor : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
}
