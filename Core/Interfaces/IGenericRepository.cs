using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);

        Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<TResult?> GetEntityWithSpecAsync<TResult>(ISpecification<T, TResult> spec);

        Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<bool> SaveAllAsync();

        Task<bool> Exists(int id);

        Task<int> CountAsync(ISpecification<T> spec);
    }
}
