using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Quyen",
                    UserName = "quyen123",
                    Email = "quyen@mail.com",
                    Address = new Address
                    {
                        FirstName = "Quyen",
                        LastName = "Dang",
                        HouseNumber = "1/1",
                        Ward = "W",
                        District = "D",
                        City = "C",
                        ZipCode = 123

                    }
                };

                await userManager.CreateAsync(user, "Admin@123");  
            }
        }
    }
}
