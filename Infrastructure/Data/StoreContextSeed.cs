using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../Infrastructure/Data./SeedData/products.json");

                var products = JsonSerializer.Deserialize<IReadOnlyList<Product>>(productsData);
                if (products is not null)
                {
                    context.AddRange(products);
                    await context.SaveChangesAsync();
                }

            }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryMethodsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");

                var deliveryMethods = JsonSerializer.Deserialize<IReadOnlyList<DeliveryMethod>>(deliveryMethodsData);
                if (deliveryMethods is not null)
                {
                    context.AddRange(deliveryMethods);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
