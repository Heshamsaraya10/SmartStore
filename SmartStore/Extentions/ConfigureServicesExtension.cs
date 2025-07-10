using FluentValidation;
using FluentValidation.AspNetCore;
using SmartStore.Application.GenaricRepository.Abstraction;
using SmartStore.Application.GenaricRepository.Implementation;
using SmartStore.Application.Repository.Abstraction;
using SmartStore.Application.Repository.Implementation;
using SmartStore.Application.ServiceManager.Abstraction;
using SmartStore.Application.ServiceManager.Implementation;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Implementation;
using SmartStore.Application.UnitOfWork.Abstraction;
using SmartStore.Application.UnitOfWork.Implementation;
using SmartStore.Application.Validators;

namespace SmartStore.Extentions
{
    public static class ConfigureServicesExtension
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();

            services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            services.AddScoped<IItemRepo, ItemRepo>();
            services.AddScoped<IItemCategoryRepo, ItemCategoryRepo>();
            services.AddScoped<IItemTypeRepo, ItemTypeRepo>();
            services.AddScoped<IItemUnitRepo, ItemUnitRepo>();
            services.AddScoped<IStoreRepo, StoreRepo>();
            services.AddScoped<IStoreItemQuantityRepo, StoreItemQuantityRepo>();
            services.AddScoped<IDamagedItemRepo, DamagedItemRepo>();
            services.AddScoped<IStockAdjustmentRepo, StockAdjustmentRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<ISupplierRepo, SupplierRepo>();
            services.AddScoped<ITransactionTypeRepo, TransactionTypeRepo>();
            services.AddScoped<ISafeRepo, SafeRepo>();
            services.AddScoped<ISafeTransactionRepo, SafeTransactionRepo>();
            services.AddScoped<IExpenseRepo, ExpenseRepo>();
            services.AddScoped<IRevenueRepo, RevenueRepo>();
            services.AddScoped<IInvoiceDetailRepo, InvoiceDetailRepo>();
            services.AddScoped<IInvoiceRepo, InvoiceRepo>();





            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IServiceManager, ServiceManager>();

            services.AddFluentValidation();
            services.AddValidatorsFromAssemblyContaining<ItemCategoryRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ItemUnitRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ItemTypeRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<StoreRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<StockAdjustmentRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<DamagedItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<CustomerRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<SupplierRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ExpenseRequestValidator>();


        }
    }
}
