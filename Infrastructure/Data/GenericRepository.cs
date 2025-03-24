using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T>(StoreContext _context) : IGenericRepository<T>
        where T : BaseEntity
    {
        private readonly DbSet<T> _db = _context.Set<T>();

        public async Task AddAsync(T entity) => await _db.AddAsync(entity);

        public void Delete(T entity) => _db.Remove(entity);

        public Task<bool> Exists(int id) => _db.AnyAsync(x => x.Id == id);

        public async Task<T?> GetByIdAsync(int id) => await _db.FindAsync(id);

        public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<TResult?> GetEntityWithSpecAsync<TResult>(ISpecification<T, TResult> spec) 
            => await ApplySecifications(spec).FirstOrDefaultAsync();

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
            => await ApplySecifications(spec).ToListAsync();

        public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec) 
            => await ApplySecifications(spec).ToListAsync();

        public async Task<bool> SaveAllAsync() => await _context.SaveChangesAsync() > 0;

        public void Update(T entity) => _db.Update(entity);

        private IQueryable<T> ApplySecifications(ISpecification<T> spec)
            => SpecificationEvaluator<T>.GetQuery(_db.AsQueryable(), spec);

        private IQueryable<TResult> ApplySecifications<TResult>(ISpecification<T, TResult> spec)
           => SpecificationEvaluator<T>.GetQuery(_db.AsQueryable(), spec);
    }
}
