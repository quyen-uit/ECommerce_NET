using System.Text.Json;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<AppUser> userManager
        )
        {
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
                    Addresses = listAddress
                };

                await userManager.CreateAsync(user, "Admin@123");
            }
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            if (!context.Categories.Any())
            {
                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/categories.json");
                var types = JsonSerializer.Deserialize<List<Category>>(typesData, jsonOptions);
                context.Categories.AddRange(types!);
            }

            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(
                    brandsData,
                    jsonOptions
                );
                context.ProductBrands.AddRange(brands!);
            }

            if (!context.Sizes.Any())
            {
                var sizeData = File.ReadAllText("../Infrastructure/Data/SeedData/sizes.json");
                var sizes = JsonSerializer.Deserialize<List<Size>>(sizeData, jsonOptions);
                context.Sizes.AddRange(sizes!);
            }

            if (!context.Colors.Any())
            {
                var colorData = File.ReadAllText("../Infrastructure/Data/SeedData/colors.json");
                var colors = JsonSerializer.Deserialize<List<Color>>(colorData, jsonOptions);
                context.Colors.AddRange(colors!);
            }

            if (!context.Products.Any())
            {
                var productData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData, jsonOptions);
                context.Products.AddRange(products!);
            }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryData = File.ReadAllText(
                    "../Infrastructure/Data/SeedData/deliveries.json"
                );
                var deliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(
                    deliveryData,
                    jsonOptions
                );
                context.DeliveryMethods.AddRange(deliveryMethod!);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
