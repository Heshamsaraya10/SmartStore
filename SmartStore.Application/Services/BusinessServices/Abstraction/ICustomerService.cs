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
    public interface ICustomerService
    {
        Task<ServiceResult> AddCustomerAsync(CustomerRequestDto request);
        Task<PaginationObject<CustomerResponseDto>> GetCustomersAsync(int pageIndex);
        Task<CustomerResponseDto> GetCustomerByIdAsync(int customerId);
        Task<PaginationObject<CustomerResponseDto>> SearchCustomerAsync(string input, int pageIndex);
        Task<ServiceResult> UpdateCustomerAsync(int customerId, CustomerRequestDto request);
        Task<ServiceResult> DeleteCustomerAsync(int customerId);
    }
}
