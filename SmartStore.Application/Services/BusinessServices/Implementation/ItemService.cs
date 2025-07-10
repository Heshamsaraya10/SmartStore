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
using SmartStore.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Services.BusinessServices.Implementation
{
    internal class ItemService(
        IUnitOfWork unitOfWork ,
        IMapper mapper ,
        IMessageService messageService ,
        IItemRepo itemRepo
        ) : IItemService
    {
        public async Task<ServiceResult> AddItemAsync(ItemRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));

            var isExists = await itemRepo.GetAsync(c => c.NameArabic == request.NameArabic && c.IsDeleted == false);

            if (isExists != null)
                return ServiceResult.Failure(messageService.GetMessage("ItemExists"));

            var item = mapper.Map<Item>(request);

            item.Barcode = BarcodeGenerator.Generate();

            await itemRepo.AddAsync(item);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
        }

        public async Task<ServiceResult> DeleteItemAsync(int itemId)
        {
            if (itemId != 0)
            {
                var item = await itemRepo
                 .GetAsync(ic => ic.ItemId == itemId && ic.IsDeleted == false);

                if (item != null)
                {
                    item.IsDeleted = true;
                    itemRepo.Update(item);
                    await unitOfWork.SaveChangesAsync();

                    return ServiceResult.Success(messageService.GetMessage("DeleteSuccessfully"));
                }
            }
            return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
        }

        public async Task<PaginationObject<ItemResponseDto>> GetDeletedItemsAsync(int pageIndex)
        {
            var query = (itemRepo.AsQueryable(i => i.IsDeleted == true))
             .OrderBy(i => i.NameArabic)
             .ProjectTo<ItemResponseDto>(mapper.ConfigurationProvider);
            return await PaginationHelper.CreateAsync(query, pageIndex);
        }

        public async Task<ItemResponseDto> GetItemByIdAsync(int itemId)
        {
            var item = await itemRepo
              .GetAsync(ic => ic.ItemId == itemId && ic.IsDeleted == false);

            if (item != null)
            {
                var itemResp = mapper.Map<ItemResponseDto>(item);
                return itemResp;
            }
            return null;
        }

        public async Task<PaginationObject<ItemResponseDto>> GetItemsAsync(int pageIndex)
        {
            var query = (itemRepo.AsQueryable(i => i.IsDeleted == false))
               .OrderBy(i => i.NameArabic)
              .ProjectTo<ItemResponseDto>(mapper.ConfigurationProvider);
            return await PaginationHelper.CreateAsync(query, pageIndex);
        }

        public async Task<ServiceResult> RestoreItemAsync(int itemId)
        {
            if (itemId == 0)
            {
                return ServiceResult.Failure(messageService.GetMessage("InvalidId"));
            }

            var item = await itemRepo
                 .GetAsync(ic => ic.ItemId == itemId && ic.IsDeleted == true);

            if (item == null)
            {
                return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
            }

            item.IsDeleted = false;
            itemRepo.Update(item);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("RestoreSuccessfully"));
        }

        public async Task<PaginationObject<ItemResponseDto>> SearchItemAsync(string input, int pageIndex)
        {
            if (!string.IsNullOrEmpty(input))
            {
                int.TryParse(input, out int id);

                var items = itemRepo.AsQueryable(ic =>
                    (ic.ItemId == id || ic.NameArabic.Contains(input) || ic.NameEnglish.Contains(input)) && ic.IsDeleted == false);

                if (items.Any())
                {
                    var res = items.OrderBy(i => i.ItemId)
                     .ProjectTo<ItemResponseDto>(mapper.ConfigurationProvider);

                    return await PaginationHelper.CreateAsync(res, pageIndex);
                }
            }
            return null;
        }

        public async Task<ServiceResult> UpdateItemAsync(int itemId, ItemRequestDto request)
        {
            if (itemId == 0)
            {
                return ServiceResult.Failure(messageService.GetMessage("InvalidId"));
            }

            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
            {
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));
            }

            var item = await itemRepo
                .GetAsync(ic => ic.ItemId == itemId && ic.IsDeleted == false);

            if (item == null)
            {
                return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
            }

            mapper.Map(request, item);
            itemRepo.Update(item);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }
    }
}
