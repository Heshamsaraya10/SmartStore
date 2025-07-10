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
    public interface ISafeService
    {
        Task<ServiceResult> AddSafeAsync(SafeRequestDto request);
        Task<PaginationObject<SafeResponseDto>> GetSafesAsync(int pageIndex);
        Task<SafeResponseDto> GetSafeByIdAsync(int safeId);
        Task<PaginationObject<SafeResponseDto>> SearchSafeAsync(string input, int pageIndex);
        Task<ServiceResult> UpdateSafeAsync(int safeId, SafeRequestDto request);
        Task<ServiceResult> DeleteSafeAsync(int safeId);

    }
}
