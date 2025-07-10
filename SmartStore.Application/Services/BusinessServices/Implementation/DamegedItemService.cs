using AutoMapper;
using SmartStore.Application.Repository.Abstraction;
using SmartStore.Application.Repository.Implementation;
using SmartStore.Application.Responses;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Implementation;
using SmartStore.Application.Services.BusinessServices.Abstraction;
using SmartStore.Application.UnitOfWork.Abstraction;
using SmartStore.Application.UnitOfWork.Implementation;
using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Services.BusinessServices.Implementation
{
    public class DamegedItemService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        IStoreItemQuantityRepo storeItemQuantityRepo,
        IDamagedItemRepo damagedItemRepo
        ) : IDamegedItemService
    {
        public async Task<ServiceResult> AddDamagedItemAsync(DamegedItemRequestDto request)
        {
            using var transaction =  damagedItemRepo.BeginTransactionAsync();

            try
            {
                var storeItem = await storeItemQuantityRepo
                    .GetAsync(s => s.ItemId == request.ItemId && s.StoreId == request.StoreId);

                if (storeItem == null || storeItem.Quantity < request.Quantity)
                    throw new Exception(messageService.GetMessage("QuantityNotFound"));

                storeItem.Quantity -= request.Quantity;
                await storeItemQuantityRepo.UpdateWithConcurrencyAsync(storeItem);

                var mappedDamagedItem = mapper.Map<DamagedItem>(request);
                mappedDamagedItem.Date = DateTime.Now;


                await damagedItemRepo.AddAsync(mappedDamagedItem);
                await unitOfWork.SaveChangesAsync();
                await storeItemQuantityRepo.CommitTransactionAsync();

                return ServiceResult.Success(messageService.GetMessage("RegistDamegedItem"));
            }
            catch {
                await storeItemQuantityRepo.RollbackTransactionAsync();
                return ServiceResult.Failure(messageService.GetMessage("FailedToRegister"));
            }
        }

        public async Task<ServiceResult> DeleteDamagedItemAsync(int damagedItemId)
        {
            var entity = await damagedItemRepo.GetAsync(i => i.DamagedItemId == damagedItemId && i.IsDeleted == false);
            if (entity == null) return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));

            entity.IsDeleted.Equals(true);
            damagedItemRepo.Update(entity);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
        }

        public async Task<ServiceResult> UpdateDamagedItemAsync(int damagedItemId, DamegedItemRequestDto request)
        {
            var item = await damagedItemRepo.GetByIdAsync(damagedItemId);
            if (item == null) return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));

            mapper.Map(request, item);
            damagedItemRepo.Update(item);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }
    }
}
