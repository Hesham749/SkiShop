using System.Collections.Concurrent;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork(StoreContext context) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, object> _repositories = [];

        public async Task<bool> CompleteAsync() => await context.SaveChangesAsync() > 0;

        public void Dispose() => context.Dispose();

        public IGenericRepository<TEntity> Repository<TEntity>()
            where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;

            return (IGenericRepository<TEntity>)_repositories.GetOrAdd(key, t =>
            {
                return new GenericRepository<TEntity>(context);
            });
        }
    }
}
