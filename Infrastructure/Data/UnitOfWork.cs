using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork(StoreContext context) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, object> _repositories = [];

        public async Task<bool> CompleteAsync() => await context.SaveChangesAsync() > 0;

        public void Dispose() => context.Dispose();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;

            return (IGenericRepository<TEntity>)_repositories.GetOrAdd(type, t =>
            {
                var repositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));

                return Activator.CreateInstance(repositoryType, context)
                ?? throw new InvalidOperationException($"Couldn't create repository instance for {t}");
            });
        }
    }
}
