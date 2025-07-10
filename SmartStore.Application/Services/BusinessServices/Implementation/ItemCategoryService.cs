using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
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
    public class ItemCategoryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageService messageService,
        IItemCategoryRepo itemCategoryRepo
        ) : IItemCategoryService
    {
        public async Task<ServiceResult> AddItemCategoryAsync(ItemCategoryRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));

            var isExists = await itemCategoryRepo
                .GetAsync(c => c.NameArabic == request.NameArabic && c.IsDeleted == false);

            if (isExists != null)
                return ServiceResult.Failure(messageService.GetMessage("ItemExists"));

            var item = mapper.Map<ItemCategory>(request);

            await itemCategoryRepo.AddAsync(item);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("RegisterSuccessfully"));
        }

        public async Task<ServiceResult> DeleteItemCategoryAsync(int itemCategoryId)
        {
            if (itemCategoryId != 0)
            {
                var itemCategory = await itemCategoryRepo
                 .GetAsync(ic => ic.ItemCategoryId == itemCategoryId && ic.IsDeleted == false);

                if (itemCategory != null)
                {
                    itemCategory.IsDeleted = true;
                    itemCategoryRepo.Update(itemCategory);
                    await unitOfWork.SaveChangesAsync();

                    return ServiceResult.Success(messageService.GetMessage("DeleteSuccessfully"));

                }
            }
            return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
        }

        public async Task<ItemCategoryResponseDto> GetItemCategoryByIdAsync(int itemCategoryId)
        {
            var itemCategory = await itemCategoryRepo
                .GetAsync(ic => ic.ItemCategoryId == itemCategoryId && ic.IsDeleted == false);

            if (itemCategory != null)
            {
                var itemCategoryResp = mapper.Map<ItemCategoryResponseDto>(itemCategory);
                return itemCategoryResp;
            }
            return null;
        }

        public async Task<PaginationObject<ItemCategoryResponseDto>> GetItemsCategoriesAsync(int pageIndex)
        {
            var query = (itemCategoryRepo.AsQueryable(i => i.IsDeleted == false))
           .OrderBy(i => i.NameArabic)
           .ProjectTo<ItemCategoryResponseDto>(mapper.ConfigurationProvider);
            return await PaginationHelper.CreateAsync(query, pageIndex);
        }

        public async Task<PaginationObject<ItemCategoryResponseDto>> SearchItemCategoryAsync(string input, int pageIndex)
        {
            if (!string.IsNullOrEmpty(input))
            {
                int.TryParse(input, out int id);

                var itemsCategories = itemCategoryRepo.AsQueryable(ic =>
                    (ic.ItemCategoryId == id || ic.NameArabic.Contains(input) || ic.NameEnglish.Contains(input)) && ic.IsDeleted == false);

                if (itemsCategories.Any())
                {
                    var item = itemsCategories.OrderBy(i => i.ItemCategoryId)
                     .ProjectTo<ItemCategoryResponseDto>(mapper.ConfigurationProvider);

                    return await PaginationHelper.CreateAsync(item, pageIndex);
                }
            }
            return null;
        }

        public async Task<ServiceResult> UpdateItemCategoryAsync(int itemCategoryId, ItemCategoryRequestDto request)
        {
            if (itemCategoryId == 0)
            {
                return ServiceResult.Failure(messageService.GetMessage("InvalidId"));
            }

            if (request == null || string.IsNullOrWhiteSpace(request.NameArabic))
            {
                return ServiceResult.Failure(messageService.GetMessage("EmptyValue"));
            }

            var itemCategory = await itemCategoryRepo
                .GetAsync(ic => ic.ItemCategoryId == itemCategoryId && ic.IsDeleted == false);

            if (itemCategory == null)
            {
                return ServiceResult.Failure(messageService.GetMessage("ValueNotFound"));
            }

            mapper.Map(request, itemCategory);
            itemCategoryRepo.Update(itemCategory);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(messageService.GetMessage("UpdateSuccessfully"));
        }
    }
}
