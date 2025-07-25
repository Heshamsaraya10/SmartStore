﻿using SmartStore.Application.GenaricRepository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.UnitOfWork.Abstraction
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangesAsync();
        IGenaricRepository<T> GetRepository<T>() where T : class;
    }
}
