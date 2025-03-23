using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
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

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products is null) return;

                context.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
