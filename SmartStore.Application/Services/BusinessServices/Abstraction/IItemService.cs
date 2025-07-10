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
    public interface IItemService
    {
        Task<ServiceResult> AddItemAsync(ItemRequestDto request);
        Task<PaginationObject<ItemResponseDto>> GetItemsAsync(int pageIndex);
        Task<ItemResponseDto> GetItemByIdAsync(int itemId);
        Task<PaginationObject<ItemResponseDto>> SearchItemAsync(string input, int pageIndex);
        Task<ServiceResult> UpdateItemAsync(int itemId, ItemRequestDto request);
        Task<ServiceResult> DeleteItemAsync(int itemId);
        Task<ServiceResult> RestoreItemAsync(int itemId);
        Task<PaginationObject<ItemResponseDto>> GetDeletedItemsAsync(int pageIndex);

    }
}
