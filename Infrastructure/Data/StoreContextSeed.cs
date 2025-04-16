using System.Text.Json;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, UserManager<AppUser>? userManager)
        {
            ArgumentNullException.ThrowIfNull(userManager);

            if (!userManager.Users.Any(u => u.UserName == "admin@test.com"))
            {
                AppUser user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "admin@test.com",
                    UserName = "admin@test.com",
                    FirstName = "Hesham",
                    LastName = "Elsayed"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Admin");
            }


            if (!context.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../Infrastructure/Data./SeedData/products.json");

                var products = JsonSerializer.Deserialize<IReadOnlyList<Product>>(productsData);
                if (products is not null)
                {
                    context.AddRange(products);
                }

            }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryMethodsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");

                var deliveryMethods = JsonSerializer.Deserialize<IReadOnlyList<DeliveryMethod>>(deliveryMethodsData);
                if (deliveryMethods is not null)
                {
                    context.AddRange(deliveryMethods);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
