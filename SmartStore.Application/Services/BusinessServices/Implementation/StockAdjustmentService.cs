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
    public class StockAdjustmentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        IStoreItemQuantityRepo storeItemQuantityRepo ,
        IStockAdjustmentRepo stockAdjustmentRepo
        ) : IStockAdjustmentService
    {
        public async Task<ServiceResult> AddStockAdjustmentAsync(StockAdjustmentRequestDto request)
        {
            using var transaction = stockAdjustmentRepo.BeginTransactionAsync();

            try
            {
                var storeItem = await storeItemQuantityRepo.GetAsync(s => s.ItemId == request.ItemId && s.StoreId == request.StoreId);
                if (storeItem == null) return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));

                var difference = request.QuantityAfter - request.QuantityBefore;

                var adjustment = new StockAdjustment
                {
                    ItemId = request.ItemId,
                    StoreId = request.StoreId,
                    QuantityBefore = request.QuantityBefore,
                    QuantityAfter = request.QuantityAfter,
                    Difference = difference,
                    Reason = request.Reason,
                    Date = DateTime.Now
                };

                await stockAdjustmentRepo.AddAsync(adjustment);
                await unitOfWork.SaveChangesAsync();
                await stockAdjustmentRepo.CommitTransactionAsync();

                return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
            }
            catch
            {
                await stockAdjustmentRepo.RollbackTransactionAsync();
                return ServiceResult.Failure(messageService.GetMessage("FailedToRegister"));
            }
        }

        public async Task<ServiceResult> DeleteStockAdjustmentAsync(int stockAdjustmentId)
        {
            var entity = await stockAdjustmentRepo.GetByIdAsync(stockAdjustmentId);
            if (entity == null) return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));

            entity.IsDeleted.Equals(true);
            stockAdjustmentRepo.Update(entity);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }

        public async Task<ServiceResult> UpdateStockAdjustmentAsync(int stockAdjustmentId, StockAdjustmentRequestDto request)
        {
            var entity = await stockAdjustmentRepo.GetByIdAsync(stockAdjustmentId);
            if (entity == null) return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));

            mapper.Map(request, entity);
            stockAdjustmentRepo.Update(entity);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }
    }
}
