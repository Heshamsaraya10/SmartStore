using SmartStore.Application.Responses;
using SmartStore.Domain.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Services.BusinessServices.Abstraction
{
    public interface IStockAdjustmentService
    {
        Task<ServiceResult> AddStockAdjustmentAsync(StockAdjustmentRequestDto request);
        Task<ServiceResult> UpdateStockAdjustmentAsync(int stockAdjustmentId, StockAdjustmentRequestDto request);
        Task<ServiceResult> DeleteStockAdjustmentAsync(int stockAdjustmentId);
    }
}
