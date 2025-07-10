using SmartStore.Application.Responses;
using SmartStore.Domain.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Services.BusinessServices.Abstraction
{
    public interface IExpenseService
    {
        Task<ServiceResult> AddExpenseAsync(ExpenseRequestDto request);
    }
}
