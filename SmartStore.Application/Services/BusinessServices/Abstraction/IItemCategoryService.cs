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
    public interface IItemCategoryService
    {
        Task<ServiceResult> AddItemCategoryAsync(ItemCategoryRequestDto request);
        Task<PaginationObject<ItemCategoryResponseDto>> GetItemsCategoriesAsync(int pageIndex);
        Task<ItemCategoryResponseDto> GetItemCategoryByIdAsync(int itemCategoryId);
        Task<PaginationObject<ItemCategoryResponseDto>> SearchItemCategoryAsync(string input, int pageIndex);
        Task<ServiceResult> UpdateItemCategoryAsync(int itemCategoryId, ItemCategoryRequestDto request);
        Task<ServiceResult> DeleteItemCategoryAsync(int itemCategoryId);
    }
}
