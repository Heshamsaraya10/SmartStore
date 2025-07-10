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
    public class ItemTypeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        IItemTypeRepo itemTypeRepo
        ) : IItemTypeService
    {
        public async Task<ServiceResult> AddItemTypeAsync(ItemTypeRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));

            var isExists = await itemTypeRepo
                .GetAsync(c => c.NameArabic == request.NameArabic && c.IsDeleted == false);

            if (isExists != null)
                return ServiceResult.Failure(messageService.GetMessage("ItemExists"));

            var mappedItemType = mapper.Map<ItemType>(request);

            await itemTypeRepo.AddAsync(mappedItemType);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
        }

        public async Task<ServiceResult> DeleteItemTypeAsync(int itemTypeId)
        {
            if (itemTypeId != 0)
            {
                var itemType = await itemTypeRepo
                 .GetAsync(ic => ic.ItemTypeId == itemTypeId && ic.IsDeleted == false);

                if (itemType != null)
                {
                    itemType.IsDeleted = true;
                    itemTypeRepo.Update(itemType);
                    await unitOfWork.SaveChangesAsync();

                    return ServiceResult.Success(messageService.GetMessage("DeleteSuccessfully"));
                }
            }
            return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
        }

        public async Task<PaginationObject<ItemTypeResponseDto>> GetItemsTypesAsync(int pageIndex)
        {
            var query = (itemTypeRepo.AsQueryable(i => i.IsDeleted == false))
                .OrderBy(i => i.NameArabic)
                .ProjectTo<ItemTypeResponseDto>(mapper.ConfigurationProvider);
            return await PaginationHelper.CreateAsync(query, pageIndex);
        }

        public async Task<ItemTypeResponseDto> GetItemTypeByIdAsync(int itemTypeId)
        {
            var itemType = await itemTypeRepo
               .GetAsync(ic => ic.ItemTypeId == itemTypeId && ic.IsDeleted == false);

            if (itemType != null)
            {
                var itemTypeRes = mapper.Map<ItemTypeResponseDto>(itemType);
                return itemTypeRes;
            }
            return null;
        }

        public async Task<PaginationObject<ItemTypeResponseDto>> SearchItemTypeAsync(string input, int pageIndex)
        {
            if (!string.IsNullOrEmpty(input))
            {
                int.TryParse(input, out int id);

                var itemsTypes = itemTypeRepo.AsQueryable(ic =>
                    (ic.ItemTypeId == id || ic.NameArabic.Contains(input) || ic.NameEnglish.Contains(input)) && ic.IsDeleted == false);

                if (itemsTypes.Any())
                {
                    var res = itemsTypes.OrderBy(i => i.ItemTypeId)
                     .ProjectTo<ItemTypeResponseDto>(mapper.ConfigurationProvider);

                    return await PaginationHelper.CreateAsync(res, pageIndex);
                }
            }
            return null;
        }

        public async Task<ServiceResult> UpdateItemTypeAsync(int itemTypeId, ItemTypeRequestDto request)
        {
            if (itemTypeId == 0)
            {
                return ServiceResult.Failure(messageService.GetMessage("InvalidId"));
            }

            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
            {
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));
            }

            var itemType = await itemTypeRepo
               .GetAsync(ic => ic.ItemTypeId == itemTypeId && ic.IsDeleted == false);

            if (itemType == null)
            {
                return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
            } 

            mapper.Map(request, itemType);
            itemTypeRepo.Update(itemType);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }
    }
} 
