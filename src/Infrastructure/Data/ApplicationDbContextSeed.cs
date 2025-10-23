using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager
        )
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new AppRole { Name = "Admin" });
                await roleManager.CreateAsync(new AppRole { Name = "User" });
            }

            if (!userManager.Users.Any())
            {
                var listAddress = new List<Address>{new Address
                {
                    FirstName = "Quyen",
                    LastName = "Dang",
                    HouseNumber = "1/1",
                    Ward = "W",
                    District = "D",
                    City = "C",
                    Street = "11",
                }};
                var user = new AppUser
                {
                    DisplayName = "Quyen",
                    UserName = "quyen123",
                    Email = "quyen@mail.com",
                    Addresses = listAddress,

                };

                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, "Admin");
            }

            // Seed permissions for all modules/actions and assign to Admin
            var actions = new[] { "Read", "Create", "Update", "Delete", "Manage" };
            var modules = Enum.GetValues<AppModule>();

            var existingPermissions = context.Permissions.ToList();
            var newPermissions = new List<Permission>();
            foreach (var module in modules)
            {
                foreach (var action in actions)
                {
                    var name = $"{module}.{action}";
                    if (!existingPermissions.Any(p => p.Name == name))
                    {
                        newPermissions.Add(new Permission
                        {
                            Name = name,
                            Description = name,
                            Module = module,
                            Action = action
                        });
                    }
                }
            }
            if (newPermissions.Count > 0)
            {
                context.Permissions.AddRange(newPermissions);
                await context.SaveChangesAsync();
                existingPermissions.AddRange(newPermissions);
            }

            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole != null)
            {
                var currentAdminPermissionIds = context.RolePermissions
                    .Where(rp => rp.RoleId == adminRole.Id)
                    .Select(rp => rp.PermissionId)
                    .ToHashSet();

                var toAssign = existingPermissions
                    .Where(p => !currentAdminPermissionIds.Contains(p.Id))
                    .Select(p => new RolePermission { RoleId = adminRole.Id, PermissionId = p.Id })
                    .ToList();

                if (toAssign.Count > 0)
                {
                    context.RolePermissions.AddRange(toAssign);
                    await context.SaveChangesAsync();
                }
            }

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Only seed independent master data; let EF generate GUID Ids
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData, jsonOptions) ?? new();
                if (brands.Count > 0) context.ProductBrands.AddRange(brands);
            }

            if (!context.Sizes.Any())
            {
                var sizeData = File.ReadAllText("../Infrastructure/Data/SeedData/sizes.json");
                var sizes = JsonSerializer.Deserialize<List<Size>>(sizeData, jsonOptions) ?? new();
                if (sizes.Count > 0) context.Sizes.AddRange(sizes);
            }

            if (!context.Colors.Any())
            {
                var colorData = File.ReadAllText("../Infrastructure/Data/SeedData/colors.json");
                var colors = JsonSerializer.Deserialize<List<Color>>(colorData, jsonOptions) ?? new();
                if (colors.Count > 0) context.Colors.AddRange(colors);
            }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryData = File.ReadAllText("../Infrastructure/Data/SeedData/deliveries.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData, jsonOptions) ?? new();
                if (deliveryMethods.Count > 0) context.DeliveryMethods.AddRange(deliveryMethods);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }

        // No seed DTOs needed when JSON omits ids and schema uses GUIDs
    }
}
