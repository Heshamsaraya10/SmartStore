using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.GenaricRepository.Abstraction
{
    public interface IGenaricRepository<T> : IGenaricRepository , IDisposable where T : class
    {
        T Add(T entity);
        Task AddAsync(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<bool> AddRangeAsync(List<T> entities);
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);

        T Update(T entity);

        Task DeleteAsync(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(object id);
        void Delete(long id);
        Task<long> CountAsync(Expression<Func<T, bool>> predict);
        long Count(Expression<Func<T, bool>> predict);
        Task<T> GetAsync(Expression<Func<T, bool>> predict);
        IQueryable<T> AsQueryable();
        IQueryable<T> AsQueryable(Expression<Func<T, bool>> predict);
        Task BeginTransactionAsync();
        Task UpdateWithConcurrencyAsync(T entity);
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }

    public interface IGenaricRepository
    {

    }
}
