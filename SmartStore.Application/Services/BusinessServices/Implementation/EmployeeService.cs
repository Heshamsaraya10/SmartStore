using AutoMapper;
using AutoMapper.QueryableExtensions;
using SmartStore.Application.Repository.Abstraction;
using SmartStore.Application.Repository.Implementation;
using SmartStore.Application.Responses;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Implementation;
using SmartStore.Application.Services.BusinessServices.Abstraction;
using SmartStore.Application.UnitOfWork.Abstraction;
using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Dtos.Response;
using SmartStore.Domain.Entities;
using SmartStore.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Services.BusinessServices.Implementation
{
    public class EmployeeService(
        IUnitOfWork unitOfWork ,
        IMapper mapper ,
        IMessageService messageService ,
        IEmployeeRepo employeeRepo)
        : IEmployeeService
    {

        public async Task<ServiceResult> AddEmployeeAsync(EmployeeRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));

            var isExists = await employeeRepo
                .GetAsync(c => c.NameArabic == request.NameArabic && c.IsDeleted == false);

            if (isExists != null)
                return ServiceResult.Failure(messageService.GetMessage("EmployeeExists"));

            var mappedEmployee = mapper.Map<Employee>(request);

            await employeeRepo.AddAsync(mappedEmployee);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
        }

        public async Task<ServiceResult> DeleteEmployeeAsync(int employeeId)
        {
            if(employeeId != 0)
            {
                var employee = await employeeRepo
                .GetAsync(ic => ic.EmployeeId == employeeId && ic.IsDeleted == false);

                if(employee != null)
                {
                    employee.IsDeleted = true;
                    employeeRepo.Update(employee);
                    await unitOfWork.SaveChangesAsync();

                    return ServiceResult.Success(messageService.GetMessage("DeleteSuccessfully"));
                }
            }
                return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
         }

        public async Task<PaginationObject<EmployeeResponseDto>> GetEmployeeAsync(int pageIndex)
        {
            var query = (employeeRepo.AsQueryable(i => i.IsDeleted == false))
                .OrderBy(i => i.NameArabic)
                .ProjectTo<EmployeeResponseDto>(mapper.ConfigurationProvider);
            return await PaginationHelper.CreateAsync(query, pageIndex);
        }

        public async Task<EmployeeResponseDto> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await employeeRepo
               .GetAsync(ic => ic.EmployeeId == employeeId && ic.IsDeleted == false);

            if (employee != null)
            {
                var employeeResponse = mapper.Map<EmployeeResponseDto>(employee);
                return employeeResponse;
            }
            return null;
        }

        public async Task<PaginationObject<EmployeeResponseDto>> SearchEmployeeAsync(string input, int pageIndex)
        {
            if (!string.IsNullOrEmpty(input))
            {
                int.TryParse(input, out int id);

                var employees = employeeRepo.AsQueryable(ic =>
                     (ic.EmployeeId == id || ic.NameArabic.Contains(input) || ic.NameEnglish.Contains(input))
                     && ic.IsDeleted == false);

                if (employees.Any())
                {
                    var res = employees.OrderBy(i => i.EmployeeId)
                    .ProjectTo<EmployeeResponseDto>(mapper.ConfigurationProvider);

                    return await PaginationHelper.CreateAsync(res, pageIndex);
                }
            }
            return null;
        }

        public async Task<ServiceResult> UpdateEmployeeAsync(int employeeId, EmployeeRequestDto request)
        {
            if (employeeId == 0)
            {
                return ServiceResult.Failure(messageService.GetMessage("InvalidId"));
            }

            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
            {
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));
            }

            var employee = await employeeRepo.GetAsync(i  => i.EmployeeId == employeeId);

            if (employee == null)
            {
                return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
            }
            mapper.Map(request, employee);
            employeeRepo.Update(employee);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }
    } 
}
