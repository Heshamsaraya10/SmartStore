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
    public interface IItemTypeService
    {
        Task<ServiceResult> AddItemTypeAsync(ItemTypeRequestDto request);
        Task<PaginationObject<ItemTypeResponseDto>> GetItemsTypesAsync(int pageIndex);
        Task<ItemTypeResponseDto> GetItemTypeByIdAsync(int itemTypeId);
        Task<PaginationObject<ItemTypeResponseDto>> SearchItemTypeAsync(string input, int pageIndex);
        Task<ServiceResult> UpdateItemTypeAsync(int itemTypeId, ItemTypeRequestDto request);
        Task<ServiceResult> DeleteItemTypeAsync(int itemTypeId);

    }
}
