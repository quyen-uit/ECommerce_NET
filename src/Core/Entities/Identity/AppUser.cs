using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public Address Address { get; set; } = default!; // Navigation property
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    }
}
