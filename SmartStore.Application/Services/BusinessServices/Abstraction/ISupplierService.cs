using SmartStore.Application.Responses;
using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Dtos.Response;
using SmartStore.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Services.BusinessServices.Abstraction
{
    public interface ISupplierService
    {
        Task<ServiceResult> AddSupplierAsync(SupplierRequestDto request);
        Task<PaginationObject<SupplierResponseDto>> GetSuppliersAsync(int pageIndex);
        Task<SupplierResponseDto> GetSupplierByIdAsync(int supplierId);
        Task<PaginationObject<SupplierResponseDto>> SearchSupplierAsync(string input, int pageIndex);
        Task<ServiceResult> UpdateSupplierAsync(int supplierId, SupplierRequestDto request);
        Task<ServiceResult> DeleteSupplierAsync(int supplierId);

    }
}
