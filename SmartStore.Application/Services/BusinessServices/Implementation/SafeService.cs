using AutoMapper;
using AutoMapper.QueryableExtensions;
using SmartStore.Application.Repository.Abstraction;
using SmartStore.Application.Responses;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Application.Services.ApplicationServices.Implementation;
using SmartStore.Application.Services.BusinessServices.Abstraction;
using SmartStore.Application.UnitOfWork.Abstraction;
using SmartStore.Application.UnitOfWork.Implementation;
using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Dtos.Response;
using SmartStore.Domain.Entities;
using SmartStore.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Services.BusinessServices.Implementation
{
    public class SafeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        ISafeRepo safeRepo
        ) : ISafeService
    {
        public async Task<ServiceResult> AddSafeAsync(SafeRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));

            var isExists = await safeRepo
                .GetAsync(c => c.NameArabic == request.NameArabic && c.IsDeleted == false);

            if (isExists != null)
                return ServiceResult.Failure(messageService.GetMessage("ItemExists"));

            var entity = mapper.Map<Safe>(request);

            await safeRepo.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
        }

        public async Task<ServiceResult> DeleteSafeAsync(int safeId)
        {
            if (safeId != 0)
            {
                var safe = await safeRepo
                 .GetAsync(ic => ic.SafeId == safeId && ic.IsDeleted == false);

                if (safe != null)
                {
                    safe.IsDeleted.Equals(true);
                    safeRepo.Update(safe);
                    await unitOfWork.SaveChangesAsync();

                    return ServiceResult.Success(messageService.GetMessage("DeleteSuccessfully"));
                }
            }
            return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
        }

        public async Task<SafeResponseDto> GetSafeByIdAsync(int safeId)
        {
            var safe = await safeRepo
                .GetAsync(ic => ic.SafeId == safeId && ic.IsDeleted == false);

            if (safe != null)
            {
                var safeResponse = mapper.Map<SafeResponseDto>(safe);
                return safeResponse;
            }
            return null;
        }

        public async Task<PaginationObject<SafeResponseDto>> GetSafesAsync(int pageIndex)
        {
            var query = (safeRepo.AsQueryable(i => i.IsDeleted == false))
                .OrderBy(i => i.NameArabic)
                .ProjectTo<SafeResponseDto>(mapper.ConfigurationProvider);
            return await PaginationHelper.CreateAsync(query, pageIndex);
        }

        public async Task<PaginationObject<SafeResponseDto>> SearchSafeAsync(string input, int pageIndex)
        {
            if (!string.IsNullOrEmpty(input))
            {
                int.TryParse(input, out int id);

                var safes = safeRepo.AsQueryable(ic =>
                    (ic.SafeId == id || ic.NameArabic.Contains(input) || ic.NameEnglish.Contains(input)) && ic.IsDeleted == false);

                if (safes.Any())
                {
                    var res = safes.OrderBy(i => i.SafeId)
                     .ProjectTo<SafeResponseDto>(mapper.ConfigurationProvider);

                    return await PaginationHelper.CreateAsync(res, pageIndex);
                }
            }
            return null;
        }

        public async Task<ServiceResult> UpdateSafeAsync(int safeId, SafeRequestDto request)
        {
            if (safeId == 0)
            {
                return ServiceResult.Failure(messageService.GetMessage("InvalidId"));
            }

            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
            {
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));
            }

            var safe = await safeRepo
                .GetAsync(ic => ic.SafeId == safeId && ic.IsDeleted == false);

            if (safe == null)
            {
                return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
            }

            mapper.Map(request, safe);
            safeRepo.Update(safe);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }
    }
}
