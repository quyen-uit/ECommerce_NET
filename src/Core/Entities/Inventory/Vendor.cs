using Core.Common;

namespace Core.Entities.Inventory
{

    public class Vendor : BaseEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
