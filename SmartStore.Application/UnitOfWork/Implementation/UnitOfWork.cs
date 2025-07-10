using SmartStore.Application.GenaricRepository.Abstraction;
using SmartStore.Application.GenaricRepository.Implementation;
using SmartStore.Application.UnitOfWork.Abstraction;
using SmartStore.Domain.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly SmartStoreContext _context;

        public UnitOfWork(SmartStoreContext context)
        {
            _context = context;
        }
        public IGenaricRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.TryGetValue(type, out var repository))
            {
                repository = new GenaricRepository<T>(_context);
                _repositories[type] = repository;
            }

            return (IGenaricRepository<T>)repository;
        }

        public async Task<int> SaveChangesAsync()
          => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
    }
}
