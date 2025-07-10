using AutoMapper;
using AutoMapper.QueryableExtensions;
using SmartStore.Application.Repository.Abstraction;
using SmartStore.Application.Repository.Implementation;
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
    public class StoreService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        IStoreRepo storeRepo
        ): IStoreService
    {
        public async Task<ServiceResult> AddStoreAsync(StoreRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));

            var isExists = await storeRepo.GetAsync(c => c.Name == request.Name && c.IsDeleted == false);

            if (isExists != null)
                return ServiceResult.Failure(messageService.GetMessage("StoreExists"));


            var store = mapper.Map<Store>(request);

            await storeRepo.AddAsync(store);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
        }

        public async Task<ServiceResult> DeleteStoreAsync(int storeId)
        {
            if (storeId != 0)
            {
                var store = await storeRepo
                    .GetAsync(ic => ic.StoreId == storeId && ic.IsDeleted == false);

                if (store != null)
                {
                    store.IsDeleted = true;
                    storeRepo.Update(store);
                    await unitOfWork.SaveChangesAsync();

                    return ServiceResult.Success(messageService.GetMessage("DeleteSuccessfully"));
                }
            }
            return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
        }

        public async Task<StoreResponseDto> GetStoreByIdAsync(int storeId)
        {
            var store = await storeRepo
            .GetAsync(ic => ic.StoreId == storeId && ic.IsDeleted == false);

            if (store != null)
            {
                var storeRes = mapper.Map<StoreResponseDto>(store);
                return storeRes;
            }
            return null;
        }

        public async Task<PaginationObject<StoreResponseDto>> GetStoresAsync(int pageIndex)
        {
            var query = (storeRepo.AsQueryable(i => i.IsDeleted == false))
              .OrderBy(i => i.Name)
              .ProjectTo<StoreResponseDto>(mapper.ConfigurationProvider);
            return await PaginationHelper.CreateAsync(query, pageIndex);
        }

        public async Task<PaginationObject<StoreResponseDto>> SearchStoreAsync(string input, int pageIndex)
        {
            if (!string.IsNullOrEmpty(input))
            {
                int.TryParse(input, out int id);

                var stores = storeRepo.AsQueryable(ic =>
                    (ic.StoreId == id || ic.Name.Contains(input)) && ic.IsDeleted == false);

                if (stores.Any())
                {
                    var res = stores.OrderBy(i => i.StoreId)
                     .ProjectTo<StoreResponseDto>(mapper.ConfigurationProvider);

                    return await PaginationHelper.CreateAsync(res, pageIndex);
                }
            }
            return null;
        }
        

        public async Task<ServiceResult> UpdateStoreAsync(int storeId, StoreRequestDto request)
        {
            if (storeId == 0)
            {
                return ServiceResult.Failure(messageService.GetMessage("InvalidId"));
            }

            if (request == null || string.IsNullOrWhiteSpace(request.Name))
            {
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));
            }
            var store = await storeRepo
               .GetAsync(ic => ic.StoreId == storeId && ic.IsDeleted == false);

            if (store == null)
            {
                return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
            }

            mapper.Map(request, store);
            storeRepo.Update(store);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }
    }
}
