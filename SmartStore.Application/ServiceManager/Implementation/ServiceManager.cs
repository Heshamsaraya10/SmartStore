using AutoMapper;
using SmartStore.Application.Repository.Abstraction;
using SmartStore.Application.Repository.Implementation;
using SmartStore.Application.ServiceManager.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Application.Services.BusinessServices.Abstraction;
using SmartStore.Application.Services.BusinessServices.Implementation;
using SmartStore.Application.UnitOfWork.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.ServiceManager.Implementation
{

    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IItemCategoryService> _itemCategoryService;
        private readonly Lazy<IItemTypeService> _itemTypeService;
        private readonly Lazy<IItemUnitService> _itemUnitService;
        private readonly Lazy<IItemService> _itemService;
        private readonly Lazy<IStoreService> _storeService;
        private readonly Lazy<IDamegedItemService> _damegedItemService;
        private readonly Lazy<IStockAdjustmentService> _stockAdjustmentService;
        private readonly Lazy<ICustomerService> _customerService;
        private readonly Lazy<ISupplierService> _supplierService;
        private readonly Lazy<ISafeService> _safeService;
        private readonly Lazy<IExpenseService> _expenseService;
        private readonly Lazy<IRevenueService> _revenueService;
        private readonly Lazy<IInvoiceService> _invoiceService;







        public ServiceManager(
            IUnitOfWork unitOfWork ,
            IMapper mapper ,
            IMessageService messageService ,
            IEmployeeRepo employeeRepo ,
            IItemCategoryRepo itemCategoryRepo ,
            IItemUnitRepo itemUnitRepo ,
            IItemTypeRepo itemTypeRepo ,
            IItemRepo itemRepo ,
            IStoreRepo storeRepo ,
            IStoreItemQuantityRepo storeItemQuantityRepo,
            IDamagedItemRepo damagedItemRepo ,
            IStockAdjustmentRepo stockAdjustmentRepo,
            ICustomerRepo customerRepo ,
            ISupplierRepo supplierRepo,
            ISafeRepo safeRepo,
            IExpenseRepo expenseRepo,
            IRevenueRepo revenueRepo,
            IInvoiceRepo invoiceRepo

            )
        {
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(unitOfWork , mapper , messageService , employeeRepo));
            _itemCategoryService = new Lazy<IItemCategoryService>(() => new ItemCategoryService(unitOfWork, mapper, messageService, itemCategoryRepo));
            _itemTypeService = new Lazy<IItemTypeService>(() => new ItemTypeService(unitOfWork, mapper, messageService, itemTypeRepo));
            _itemUnitService = new Lazy<IItemUnitService>(() => new ItemUnitService(unitOfWork, mapper, messageService, itemUnitRepo));
            _itemService = new Lazy<IItemService>(() => new ItemService(unitOfWork, mapper, messageService, itemRepo));
            _storeService = new Lazy<IStoreService>(() => new StoreService(unitOfWork, mapper, messageService, storeRepo));
            _damegedItemService = new Lazy<IDamegedItemService>(() => new DamegedItemService(unitOfWork, mapper, messageService, storeItemQuantityRepo ,damagedItemRepo));
            _stockAdjustmentService = new Lazy<IStockAdjustmentService>(() => new StockAdjustmentService(unitOfWork, mapper, messageService, storeItemQuantityRepo, stockAdjustmentRepo));
            _customerService = new Lazy<ICustomerService>(() => new CustomerService(unitOfWork, mapper, messageService, customerRepo));
            _supplierService = new Lazy<ISupplierService>(() => new SupplierService(unitOfWork, mapper, messageService, supplierRepo));
            _safeService = new Lazy<ISafeService>(() => new SafeService(unitOfWork, mapper, messageService, safeRepo));
            _expenseService = new Lazy<IExpenseService>(() => new ExpenseService(unitOfWork, mapper, messageService, expenseRepo , safeRepo));
            _revenueService = new Lazy<IRevenueService>(() => new RevenueService(unitOfWork, mapper, messageService, revenueRepo, safeRepo));
            _invoiceService = new Lazy<IInvoiceService>(() => new InvoiceService(unitOfWork, mapper, messageService, invoiceRepo, customerRepo, safeRepo, supplierRepo, storeItemQuantityRepo));

        }

        public IEmployeeService EmployeeService => _employeeService.Value;
        public IItemCategoryService itemCategoryService => _itemCategoryService.Value;
        public IItemTypeService ItemTypeService => _itemTypeService.Value;
        public IItemUnitService ItemUnitService => _itemUnitService.Value;
        public IItemService ItemService => _itemService.Value;
        public IStoreService StoreService => _storeService.Value;
        public IDamegedItemService DamegedItemService => _damegedItemService.Value;
        public IStockAdjustmentService StockAdjustmentService => _stockAdjustmentService.Value;
        public ICustomerService CustomerService => _customerService.Value;
        public ISupplierService SupplierService => _supplierService.Value;
        public ISafeService SafeService => _safeService.Value;
        public IExpenseService expenseService => _expenseService.Value;
        public IRevenueService RevenueService => _revenueService.Value;

        public IInvoiceService InvoiceService => _invoiceService.Value;
    }
}
