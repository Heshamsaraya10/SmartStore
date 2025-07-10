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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SmartStore.Application.Services.BusinessServices.Implementation
{
    public class SupplierService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        ISupplierRepo supplierRepo
        ) : ISupplierService
    {
        public async Task<ServiceResult> AddSupplierAsync(SupplierRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));

            var isExists = await supplierRepo
                .GetAsync(c => c.NameArabic == request.NameArabic && c.IsDeleted == false);

            if (isExists != null)
                return ServiceResult.Failure(messageService.GetMessage("ItemExists"));

            var entity = mapper.Map<Supplier>(request);

            await supplierRepo.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
        }

        public async Task<ServiceResult> DeleteSupplierAsync(int supplierId)
        {
            if (supplierId != 0)
            {
                var supplier = await supplierRepo
                 .GetAsync(ic => ic.SupplierId == supplierId && ic.IsDeleted == false);

                if (supplier != null)
                {
                    supplier.IsDeleted = true;
                    supplierRepo.Update(supplier);
                    await unitOfWork.SaveChangesAsync();

                    return ServiceResult.Success(messageService.GetMessage("DeleteSuccessfully"));
                }
            }
            return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
        }

        public async Task<SupplierResponseDto> GetSupplierByIdAsync(int supplierId)
        {
            var item = await supplierRepo
                .GetAsync(ic => ic.SupplierId == supplierId && ic.IsDeleted == false);

            if (item != null)
            {
                var itemResp = mapper.Map<SupplierResponseDto>(item);
                return itemResp;
            }
            return null;
        }
        

        public async Task<PaginationObject<SupplierResponseDto>> GetSuppliersAsync(int pageIndex)
        {
             var query = (supplierRepo.AsQueryable(i => i.IsDeleted == false))
            .OrderBy(i => i.NameArabic)
            .ProjectTo<SupplierResponseDto>(mapper.ConfigurationProvider);
            return await PaginationHelper.CreateAsync(query, pageIndex);
        }

        public async Task<PaginationObject<SupplierResponseDto>> SearchSupplierAsync(string input, int pageIndex)
        {
            if (!string.IsNullOrEmpty(input))
            {
                int.TryParse(input, out int id);

                var suppliers = supplierRepo.AsQueryable(ic =>
                    (ic.SupplierId == id || ic.NameArabic.Contains(input) || ic.NameEnglish.Contains(input)) && ic.IsDeleted == false);

                if (suppliers.Any())
                {
                    var res = suppliers.OrderBy(i => i.SupplierId)
                     .ProjectTo<SupplierResponseDto>(mapper.ConfigurationProvider);

                    return await PaginationHelper.CreateAsync(res, pageIndex);
                }
            }
            return null;
        }

        public async Task<ServiceResult> UpdateSupplierAsync(int supplierId, SupplierRequestDto request)
        {
            if (supplierId == 0)
            {
                return ServiceResult.Failure(messageService.GetMessage("InvalidId"));
            }

            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
            {
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));
            }

            var supplier = await supplierRepo
                .GetAsync(ic => ic.SupplierId == supplierId && ic.IsDeleted == false);

            if (supplier == null)
            {
                return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
            }

            mapper.Map(request, supplier);
            supplierRepo.Update(supplier);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }
    
    }
}
