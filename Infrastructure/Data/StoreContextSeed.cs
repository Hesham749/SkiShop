using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedProductsAsync(StoreContext context)
        {
            if (!context.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../Infrastructure/Data./SeedData/products.json");

                var products = JsonSerializer.Deserialize<IReadOnlyList<Product>>(productsData);
                if (products is null) return;

                context.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
