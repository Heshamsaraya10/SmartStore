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
    public interface IStoreService
    {
        Task<ServiceResult> AddStoreAsync(StoreRequestDto request);
        Task<PaginationObject<StoreResponseDto>> GetStoresAsync(int pageIndex);
        Task<StoreResponseDto> GetStoreByIdAsync(int storeId);
        Task<PaginationObject<StoreResponseDto>> SearchStoreAsync(string input, int pageIndex);
        Task<ServiceResult> UpdateStoreAsync(int storeId, StoreRequestDto request);
        Task<ServiceResult> DeleteStoreAsync(int storeId);

    }
}
 