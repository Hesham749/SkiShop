using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository(StoreContext _context) : IProductRepository
    {
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Remove(product);
        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await _context.Products
                .Select(p => p.Brand)
                .Distinct()
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
        {
            var query = _context.Products.AsQueryable();
            query = query.Where(p => p.Brand == brand || string.IsNullOrWhiteSpace(brand));
            query = query.Where(p => p.Brand == type || string.IsNullOrWhiteSpace(type));
            query = sort switch
            {
                "priceAsc" or "price" or "priceasc" => query.OrderBy(p => p.Price),
                "priceDesc" or "pricedesc" => query?.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await _context.Products
                .Select(p => p.Type)
                .Distinct()
                .ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            _context.Update(product);
        }
    }
}
