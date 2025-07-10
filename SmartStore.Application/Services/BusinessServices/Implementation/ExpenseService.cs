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
    public class ExpenseService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        IExpenseRepo expenseRepo,
        ISafeRepo safeRepo
        ) : IExpenseService
    {
        public async Task<ServiceResult> AddExpenseAsync(ExpenseRequestDto request)
        {
            using var transaction = expenseRepo.BeginTransactionAsync();

            try
            {
                var safe = await safeRepo.GetAsync(i => i.IsDeleted == false);
                await safeRepo.UpdateWithConcurrencyAsync(safe);

                if (safe.Balance < request.Amount)
                    throw new Exception(messageService.GetMessage("InsufficientBalance"));

                safe.Balance -= request.Amount;
                var safeTransaction = new SafeTransaction
                {
                    SafeId = safe.SafeId,
                    Amount = request.Amount,
                    TransactionTypeId = 5, // نوع: مصروف
                    Date = request.Date,
                    Description = request.Notes
                };

                request.SafeTransaction = safeTransaction;
                var expense = mapper.Map<Expense>(request);
                await expenseRepo.AddAsync(expense);


                await unitOfWork.SaveChangesAsync();
                await expenseRepo.CommitTransactionAsync();

                return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
            }
            catch 
            {
                await expenseRepo.RollbackTransactionAsync();
                return ServiceResult.Failure(messageService.GetMessage("CanNotExpenseRegist"));
            }
        }
    }
}
