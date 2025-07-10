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
    public class RevenueService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        IRevenueRepo revenueRepo,
        ISafeRepo safeRepo
        ) : IRevenueService
    {
        public async Task<ServiceResult> AddRevenueAsync(RevenueRequestDto request)
        {
            using var transaction = revenueRepo.BeginTransactionAsync();

            try
            {
                var safe = await safeRepo.GetAsync(i => i.IsDeleted == false);
                await safeRepo.UpdateWithConcurrencyAsync(safe);

                if (safe.Balance < request.Amount)
                    throw new Exception(messageService.GetMessage("InsufficientBalance"));

                safe.Balance += request.Amount;

                var safeTransaction = new SafeTransaction
                {
                    SafeId = safe.SafeId,
                    Amount = request.Amount,
                    TransactionTypeId = 6, // نوع: وارد
                    Date = request.Date,
                    Description = request.Notes
                };
                request.SafeTransaction = safeTransaction;
                var expense = mapper.Map<Revenue>(request);
                await revenueRepo.AddAsync(expense);

                await unitOfWork.SaveChangesAsync();
                await revenueRepo.CommitTransactionAsync();

                return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
            }
            catch
            {
                await revenueRepo.RollbackTransactionAsync();
                return ServiceResult.Failure(messageService.GetMessage("CanNotExpenseRegist"));
            }
        }
    }
}
