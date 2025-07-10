using SmartStore.Application.Repository.Abstraction;
using SmartStore.Application.Services.BusinessServices.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.ServiceManager.Abstraction
{
    public interface IServiceManager
    {
        public IEmployeeService EmployeeService { get; }
        public IItemCategoryService itemCategoryService { get; }
        public IItemTypeService ItemTypeService { get; }
        public IItemUnitService ItemUnitService { get; }
        public IItemService ItemService { get; }
        public IStoreService StoreService { get; }
        public IDamegedItemService DamegedItemService { get; }
        public IStockAdjustmentService StockAdjustmentService { get; }
        public ICustomerService CustomerService { get; }
        public ISupplierService SupplierService { get; }
        public ISafeService SafeService { get; }
        public IExpenseService expenseService { get; }
        public IRevenueService RevenueService { get; }
        public IInvoiceService InvoiceService { get; }









    }
}
