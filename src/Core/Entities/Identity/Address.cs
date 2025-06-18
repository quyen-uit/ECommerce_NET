using System.ComponentModel.DataAnnotations;
using Core.Common.Entities;

namespace Core.Entities.Identity
{
    public class Address : AuditableEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string AppUserId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; } = default!;
    }
}