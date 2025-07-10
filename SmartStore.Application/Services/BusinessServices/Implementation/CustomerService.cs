using AutoMapper;
using AutoMapper.QueryableExtensions;
using SmartStore.Application.Repository.Abstraction;
using SmartStore.Application.Responses;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Implementation;
using SmartStore.Application.Services.BusinessServices.Abstraction;
using SmartStore.Application.UnitOfWork.Abstraction;
using SmartStore.Application.UnitOfWork.Implementation;
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
    public class CustomerService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        ICustomerRepo customerRepo
        ) : ICustomerService
    {
        public async Task<ServiceResult> AddCustomerAsync(CustomerRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));

            var isExists = await customerRepo
                .GetAsync(c => c.NameArabic == request.NameArabic && c.IsDeleted == false);

            if (isExists != null)
                return ServiceResult.Failure(messageService.GetMessage("ItemExists"));

            var customer = mapper.Map<Customer>(request);

            await customerRepo.AddAsync(customer);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));

        }

        public async Task<ServiceResult> DeleteCustomerAsync(int customerId)
        {
            if (customerId != 0)
            {
                var customer = await customerRepo
                 .GetAsync(ic => ic.CustomerId == customerId && ic.IsDeleted == false);

                if (customer != null)
                {
                    customer.IsDeleted = true;
                    customerRepo.Update(customer);
                    await unitOfWork.SaveChangesAsync();

                    return ServiceResult.Success(messageService.GetMessage("DeleteSuccessfully"));
                }
            }
            return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
        }

        public async Task<CustomerResponseDto> GetCustomerByIdAsync(int customerId)
        {
            var item = await customerRepo
                .GetAsync(ic => ic.CustomerId == customerId && ic.IsDeleted == false);

            if (item != null)
            {
                var itemResp = mapper.Map<CustomerResponseDto>(item);
                return itemResp;
            }
            return null;
        
        }

        public async Task<PaginationObject<CustomerResponseDto>> GetCustomersAsync(int pageIndex)
        {
            var query = (customerRepo.AsQueryable(i => i.IsDeleted == false))
                .OrderBy(i => i.NameArabic)
                .ProjectTo<CustomerResponseDto>(mapper.ConfigurationProvider);
            return await PaginationHelper.CreateAsync(query, pageIndex);
        }

        public async Task<PaginationObject<CustomerResponseDto>> SearchCustomerAsync(string input, int pageIndex)
        {
            if (!string.IsNullOrEmpty(input))
            {
                int.TryParse(input, out int id);

                var customers = customerRepo.AsQueryable(ic =>
                    (ic.CustomerId == id || ic.NameArabic.Contains(input) || ic.NameEnglish.Contains(input)) && ic.IsDeleted == false);

                if (customers.Any())
                {
                    var res = customers.OrderBy(i => i.CustomerId)
                     .ProjectTo<CustomerResponseDto>(mapper.ConfigurationProvider);

                    return await PaginationHelper.CreateAsync(res, pageIndex);
                }
            }
            return null;
        }

        public async Task<ServiceResult> UpdateCustomerAsync(int customerId, CustomerRequestDto request)
        {
            if (customerId == 0)
            {
                return ServiceResult.Failure(messageService.GetMessage("InvalidId"));
            }

            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
            {
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));
            }

            var customer = await customerRepo
                .GetAsync(ic => ic.CustomerId == customerId && ic.IsDeleted == false);

            if (customer == null)
            {
                return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
            }

            mapper.Map(request, customer);
            customerRepo.Update(customer);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }
    }
}
