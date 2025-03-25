
using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class CartService(IConnectionMultiplexer redis) : ICartService
    {
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task<bool> DeleteCartAsync(string key) => await _database.KeyDeleteAsync(key);

        public async Task<ShoppingCart> GetCartAsync(string key)
        {
            var cart = await _database.StringGetAsync(key);

            return !cart.IsNullOrEmpty ? JsonSerializer.Deserialize<ShoppingCart>(cart!)!
                : new ShoppingCart { Id = key };
        }

        public async Task<ShoppingCart> SetCartAsync(ShoppingCart cart)
        {
            var created = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart),
                TimeSpan.FromDays(30));

            return await GetCartAsync(cart.Id);
        }
    }
}
