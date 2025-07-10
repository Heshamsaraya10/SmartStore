using Microsoft.EntityFrameworkCore;
using SmartStore.Application.GenaricRepository.Abstraction;
using SmartStore.Domain;
using SmartStore.Domain.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.GenaricRepository.Implementation
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : class
    {
        private readonly SmartStoreContext _context;
        private readonly DbSet<T> _dbSet;

        public GenaricRepository(SmartStoreContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }


        public T Add(T entity)
        {
           var result = _dbSet.Add(entity);
            return result.Entity;
        }

        public async Task AddAsync(T entity)
             => await _dbSet.AddAsync(entity);

        public  IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            return entities;
        }

        public async Task<bool> AddRangeAsync(List<T> entities)
        {
                await _dbSet.AddRangeAsync(entities);
                return true;
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> AsQueryable(Expression<Func<T, bool>> predict)
        {
            return _dbSet.AsQueryable().Where(predict);
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();

        }

        public long Count(Expression<Func<T, bool>> predict)
        {
            return _dbSet.Where(predict).Count();
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> predict)
        {
            return await _dbSet.Where(predict).CountAsync();
        }

        public void Delete(T entity)
        {
            var exist = _dbSet.Find(entity);
            if (exist == null)
                throw new Exception("Entity not found");
            _dbSet.Remove(exist);
        }

        public void Delete(object id)
        {
            var exist = _dbSet.Find(id);
            if (exist == null)
                throw new Exception("Entity not found");
            _dbSet.Remove(exist);
        }

        public void Delete(long id)
        {
            var exist = _dbSet.Find(id);

            if (exist == null)
                throw new Exception("Entity not found");

            _dbSet.Remove(exist);
        }

        public async Task DeleteAsync(T entity)
        {
            var exist = await _dbSet.FindAsync(entity);
            if (exist == null)
                throw new Exception("Entity not found");
            _dbSet.Remove(exist);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predict)
        {
            return await _dbSet.AsNoTracking().Where(predict).FirstOrDefaultAsync(predict);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public T Update(T entity)
        {
            var result = _dbSet.Update(entity);
            return result.Entity;
        }

        public async Task UpdateWithConcurrencyAsync(T entity)
        {
            if (entity is not IHasRowVersion rowVersionEntity)
                throw new InvalidOperationException("This entity does not support concurrency.");

            _context.Entry(entity).Property("RowVersion").OriginalValue = rowVersionEntity.RowVersion;

            _context.Set<T>().Update(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("تم تعديل العنصر من مستخدم آخر. يرجى تحديث الصفحة والمحاولة مجددًا.");
            }
        }
    }
}
