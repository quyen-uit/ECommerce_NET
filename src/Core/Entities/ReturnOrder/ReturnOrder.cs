using Core.Common;
using Core.Entities.OrderAggregate;
using Core.Enums;

namespace Core.Entities.ReturnOrder
{
    public class ReturnOrder : AuditableEntity
    {
        public long OrderId { get; set; }
        public DateTime ReturnDate { get; set; } = DateTime.UtcNow;
        public string? Reason { get; set; }
        public ReturnOrderStatus Status { get; set; }
        public string? Notes { get; set; }
        public decimal TotalAmount { get; set; }
        public Order Order { get; set; } = default!; // Navigation property
    }
}
