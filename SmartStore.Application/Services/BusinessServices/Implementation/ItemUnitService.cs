using AutoMapper;
using AutoMapper.QueryableExtensions;
using SmartStore.Application.Repository.Abstraction;
using SmartStore.Application.Repository.Implementation;
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
    public class ItemUnitService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        IItemUnitRepo itemUnitRepo
        ) : IItemUnitService
    {
        public async Task<ServiceResult> AddItemUnitAsync(ItemUnitRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));

            var isExists = await itemUnitRepo
                .GetAsync(c => c.NameArabic == request.NameArabic && c.IsDeleted == false);

            if (isExists != null)
                return ServiceResult.Failure(messageService.GetMessage("ItemUnitExists"));

            var itemUnit = mapper.Map<ItemUnit>(request);

            await itemUnitRepo.AddAsync(itemUnit);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
        }

        public async Task<ServiceResult> DeleteItemUnitAsync(int itemUnitId)
        {
            if (itemUnitId != 0)
            {
                var itemUnit = await itemUnitRepo
                 .GetAsync(ic => ic.ItemUnitId == itemUnitId && ic.IsDeleted == false);

                if (itemUnit != null)
                {
                    itemUnit.IsDeleted = true;
                    itemUnitRepo.Update(itemUnit);
                    await unitOfWork.SaveChangesAsync();

                    return ServiceResult.Success(messageService.GetMessage("DeleteSuccessfully"));
                }
            }
            return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
        }

        public async Task<PaginationObject<ItemUnitResponseDto>> GetItemsUnitsAsync(int pageIndex)
        {
            var query = (itemUnitRepo.AsQueryable(i => i.IsDeleted == false))
            .OrderBy(i => i.NameArabic)
            .ProjectTo<ItemUnitResponseDto>(mapper.ConfigurationProvider);
            return await PaginationHelper.CreateAsync(query, pageIndex);
        }

        public async Task<ItemUnitResponseDto> GetItemUnitByIdAsync(int itemUnitId)
        {
            var itemUnit = await itemUnitRepo
               .GetAsync(ic => ic.ItemUnitId == itemUnitId && ic.IsDeleted == false);

            if (itemUnit != null)
            {
                var itemUnitResp = mapper.Map<ItemUnitResponseDto>(itemUnit);
                return itemUnitResp;
            }
            return null;
        }

        public async Task<PaginationObject<ItemUnitResponseDto>> SearchItemUnitAsync(string input, int pageIndex)
        {
            if (!string.IsNullOrEmpty(input))
            {
                int.TryParse(input, out int id);

                var itemsUnits = itemUnitRepo.AsQueryable(ic =>
                    (ic.ItemUnitId == id || ic.NameArabic.Contains(input) || ic.NameEnglish.Contains(input)) && ic.IsDeleted == false);

                if (itemsUnits.Any())
                {
                    var res = itemsUnits.OrderBy(i => i.ItemUnitId)
                     .ProjectTo<ItemUnitResponseDto>(mapper.ConfigurationProvider);

                    return await PaginationHelper.CreateAsync(res, pageIndex);
                }
            }
            return null;
        }

        public async Task<ServiceResult> UpdateItemUnitAsync(int itemUnitId, ItemUnitRequestDto request)
        {
            if (itemUnitId == 0)
            {
                return ServiceResult.Failure(messageService.GetMessage("InvalidId"));
            }

            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
            {
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));
            }

            var itemUnit = await itemUnitRepo
                .GetAsync(ic => ic.ItemUnitId == itemUnitId && ic.IsDeleted == false);

            if (itemUnit == null)
            {
                return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
            }

            mapper.Map(request, itemUnit);
            itemUnitRepo.Update(itemUnit);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }
    }
}
