using SmartStore.Application.Responses;
using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Dtos.Response;
using SmartStore.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Services.BusinessServices.Abstraction
{
    public interface IItemUnitService
    {
        Task<ServiceResult> AddItemUnitAsync(ItemUnitRequestDto request);
        Task<PaginationObject<ItemUnitResponseDto>> GetItemsUnitsAsync(int pageIndex);
        Task<ItemUnitResponseDto> GetItemUnitByIdAsync(int itemUnitId);
        Task<PaginationObject<ItemUnitResponseDto>> SearchItemUnitAsync(string input, int pageIndex);
        Task<ServiceResult> UpdateItemUnitAsync(int itemUnitId, ItemUnitRequestDto request);
        Task<ServiceResult> DeleteItemUnitAsync(int itemUnitId);

    }
}
