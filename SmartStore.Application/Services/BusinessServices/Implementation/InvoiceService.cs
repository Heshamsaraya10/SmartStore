using AutoMapper;
using SmartStore.Application.Repository.Abstraction;
using SmartStore.Application.Repository.Implementation;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Implementation;
using SmartStore.Application.Services.BusinessServices.Abstraction;
using SmartStore.Application.UnitOfWork.Abstraction;
using SmartStore.Application.UnitOfWork.Implementation;
using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Dtos.Response;
using SmartStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Services.BusinessServices.Implementation
{
    public class InvoiceService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        IInvoiceRepo invoiceRepo,
        ICustomerRepo customerRepo,
        ISafeRepo safeRepo,
        ISupplierRepo supplierRepo,
        IStoreItemQuantityRepo storeItemQuantityRepo
        ) : IInvoiceService
    {
        public async Task<InvoiceResponseDto> AddPurchaseInvoiceAsync(InvoiceRequestDto request)
        {
            using var transaction = invoiceRepo.BeginTransactionAsync();

            try
            {
                foreach (var detail in request.InvoiceDetails)
                {
                    var storeItem = await storeItemQuantityRepo
                    .GetAsync(s => s.ItemId == detail.ItemId && s.StoreId == request.StoreId);

                    if (storeItem == null || storeItem.Quantity < detail.Quantity)
                        throw new Exception(messageService.GetMessage("QuantityNotFound"));

                    storeItem.Quantity -= detail.Quantity;
                }

                var safe = await safeRepo.GetAsync(i => i.IsDeleted == false);
                await safeRepo.UpdateWithConcurrencyAsync(safe);

                safe.Balance += request.PaidAmount;

                var safeTransaction = new SafeTransaction
                {
                    SafeId = safe.SafeId,
                    Amount = request.PaidAmount,
                    TransactionTypeId = 1,
                    Date = DateTime.Now,
                    Description = "فاتورة بيع",
                };
                request.SafeTransaction = safeTransaction;
                var invoice = mapper.Map<Invoice>(request);
                await invoiceRepo.AddAsync(invoice);
                await unitOfWork.SaveChangesAsync();

                await invoiceRepo.CommitTransactionAsync();
                var customer = await customerRepo.GetAsync(i => i.CustomerId == invoice.CustomerId);

                var invoiceDto = new InvoiceResponseDto
                {
                    InvoiceId = invoice.InvoiceId,
                    StoreId = invoice.StoreId,
                    Date = invoice.Date,
                    CustomerName = customer.NameArabic,
                    TotalAmount = invoice.TotalAmount,
                    PaidAmount = invoice.PaidAmount,
                    RemainingAmount = invoice.RemainingAmount,
                    Details = invoice.InvoiceDetails.Select(d => new InvoiceDetailResponseDto
                    {
                        ItemId = d.ItemId,
                        ItemName = d.Item.NameArabic,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                        Total = d.Total
                    }).ToList()
                };

                return invoiceDto;
            }
            catch
            {
                await invoiceRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<InvoiceResponseDto> AddPurchaseReturnInvoiceAsync(InvoiceRequestDto request)
        {
            using var transaction = invoiceRepo.BeginTransactionAsync();
            try
            {
                foreach (var detail in request.InvoiceDetails)
                {
                    var storeItem = await storeItemQuantityRepo
                        .GetAsync(s => s.ItemId == detail.ItemId && s.StoreId == request.StoreId);

                    storeItem.Quantity -= detail.Quantity;
                }
                var safe = await safeRepo.GetAsync(i => i.IsDeleted == false);
                await safeRepo.UpdateWithConcurrencyAsync(safe);

                safe.Balance += request.PaidAmount;

                var safeTransaction = new SafeTransaction
                {
                    SafeId = safe.SafeId,
                    Amount = request.PaidAmount,
                    TransactionTypeId = 4,
                    Date = DateTime.Now,
                    Description = "فاتورة مرتجع شراء",
                };

                request.SafeTransaction = safeTransaction;
                var invoice = mapper.Map<Invoice>(request);
                await invoiceRepo.AddAsync(invoice);
                await unitOfWork.SaveChangesAsync();

                await invoiceRepo.CommitTransactionAsync();
                var supplier = await supplierRepo.GetAsync(i => i.SupplierId == invoice.SupplierId);

                var invoiceDto = new InvoiceResponseDto
                {
                    InvoiceId = invoice.InvoiceId,
                    StoreId = invoice.StoreId,
                    Date = invoice.Date,
                    SupplierName = supplier.NameArabic,
                    TotalAmount = invoice.TotalAmount,
                    PaidAmount = invoice.PaidAmount,
                    RemainingAmount = invoice.RemainingAmount,
                    Details = invoice.InvoiceDetails.Select(d => new InvoiceDetailResponseDto
                    {
                        ItemId = d.ItemId,
                        ItemName = d.Item.NameArabic,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                        Total = d.Total
                    }).ToList()
                };

                return invoiceDto;
            }
            catch
            {
                await invoiceRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<InvoiceResponseDto> AddSaleInvoiceAsync(InvoiceRequestDto request)
        {
            using var transaction = invoiceRepo.BeginTransactionAsync();
            try
            {
                foreach (var detail in request.InvoiceDetails)
                {
                    var storeItem = await storeItemQuantityRepo
                        .GetAsync(s => s.ItemId == detail.ItemId && s.StoreId == request.StoreId);

                    if (storeItem == null || storeItem.Quantity < detail.Quantity)
                        throw new Exception(messageService.GetMessage("QuantityNotFound"));

                    storeItem.Quantity -= detail.Quantity;
                }

                var safe = await safeRepo.GetAsync(i => i.IsDeleted == false);
                await safeRepo.UpdateWithConcurrencyAsync(safe);

                safe.Balance += request.PaidAmount;

                var safeTransaction = new SafeTransaction
                {
                    SafeId = safe.SafeId,
                    Amount = request.PaidAmount,
                    TransactionTypeId = 1,
                    Date = DateTime.Now,
                    Description = "فاتورة بيع",
                };

                request.SafeTransaction = safeTransaction;
                var invoice = mapper.Map<Invoice>(request);
                await invoiceRepo.AddAsync(invoice);
                await unitOfWork.SaveChangesAsync();

                await invoiceRepo.CommitTransactionAsync();
                var customer = await customerRepo.GetAsync(i => i.CustomerId == invoice.CustomerId);

                var invoiceDto = new InvoiceResponseDto
                {
                    InvoiceId = invoice.InvoiceId,
                    StoreId = invoice.StoreId,
                    Date = invoice.Date,
                    CustomerName = customer.NameArabic,
                    TotalAmount = invoice.TotalAmount,
                    PaidAmount = invoice.PaidAmount,
                    RemainingAmount = invoice.RemainingAmount,
                    Details = invoice.InvoiceDetails.Select(d => new InvoiceDetailResponseDto
                    {
                        ItemId = d.ItemId,
                        ItemName = d.Item.NameArabic,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                        Total = d.Total
                    }).ToList()
                };

                return invoiceDto;
            }
            catch
            {
                {
                    await invoiceRepo.RollbackTransactionAsync();
                    throw;
                }
            }

        }

        public async Task<InvoiceResponseDto> AddSalesReturnInvoiceAsync(InvoiceRequestDto request)
        {
            using var transaction = invoiceRepo.BeginTransactionAsync();

            try
            {

                foreach (var detail in request.InvoiceDetails)
                {
                    var storeItem = await storeItemQuantityRepo
                        .GetAsync(s => s.ItemId == detail.ItemId && s.StoreId == request.StoreId);
                    if (storeItem == null || storeItem.Quantity < detail.Quantity)
                        throw new Exception(messageService.GetMessage("QuantityNotFound"));

                    storeItem.Quantity += detail.Quantity;
                }


                var safe = await safeRepo.GetAsync(i => i.IsDeleted == false);
                await safeRepo.UpdateWithConcurrencyAsync(safe);

                safe.Balance -= request.PaidAmount;

                var safeTransaction = new SafeTransaction
                {
                    SafeId = safe.SafeId,
                    Amount = request.PaidAmount,
                    TransactionTypeId = 3,
                    Date = DateTime.Now,
                    Description = "فاتورة مرتجع بيع",
                };

                request.SafeTransaction = safeTransaction;
                var invoice = mapper.Map<Invoice>(request);
                await invoiceRepo.AddAsync(invoice);
                await unitOfWork.SaveChangesAsync();

                await invoiceRepo.CommitTransactionAsync();
                var customer = await customerRepo.GetAsync(i => i.CustomerId == invoice.CustomerId);

                var invoiceDto = new InvoiceResponseDto
                {
                    InvoiceId = invoice.InvoiceId,
                    StoreId = invoice.StoreId,
                    Date = invoice.Date,
                    CustomerName = customer.NameArabic,
                    TotalAmount = invoice.TotalAmount,
                    PaidAmount = invoice.PaidAmount,
                    RemainingAmount = invoice.RemainingAmount,
                    Details = invoice.InvoiceDetails.Select(d => new InvoiceDetailResponseDto
                    {
                        ItemId = d.ItemId,
                        ItemName = d.Item.NameArabic,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                        Total = d.Total
                    }).ToList()
                };

                return invoiceDto;
            }
            catch
            {
                await invoiceRepo.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
