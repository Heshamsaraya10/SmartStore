using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Services.BusinessServices.Abstraction
{
    public interface IInvoiceService
    {
        Task<InvoiceResponseDto> AddSaleInvoiceAsync(InvoiceRequestDto request);
        Task<InvoiceResponseDto> AddPurchaseInvoiceAsync(InvoiceRequestDto request);
        Task<InvoiceResponseDto> AddSalesReturnInvoiceAsync(InvoiceRequestDto request);
        Task<InvoiceResponseDto> AddPurchaseReturnInvoiceAsync(InvoiceRequestDto request);
    }
}
