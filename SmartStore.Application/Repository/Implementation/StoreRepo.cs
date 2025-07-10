using SmartStore.Application.GenaricRepository.Implementation;
using SmartStore.Application.Repository.Abstraction;
using SmartStore.Domain.Context;
using SmartStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Repository.Implementation
{
    public class StoreRepo : GenaricRepository<Store> , IStoreRepo
    {
        public StoreRepo(SmartStoreContext smartStoreContext) : base(smartStoreContext) 
        {
            
        }
    }
}
