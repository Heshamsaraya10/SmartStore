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
    public interface IEmployeeService
    {
        Task<ServiceResult> AddEmployeeAsync(EmployeeRequestDto request);
        Task<PaginationObject<EmployeeResponseDto>> GetEmployeeAsync(int pageIndex);
        Task<EmployeeResponseDto> GetEmployeeByIdAsync(int employeeId);
        Task<ServiceResult> UpdateEmployeeAsync(int employeeId, EmployeeRequestDto request);
        Task<PaginationObject<EmployeeResponseDto>> SearchEmployeeAsync(string input, int pageIndex);
        Task<ServiceResult> DeleteEmployeeAsync(int employeeId);


    }
}
