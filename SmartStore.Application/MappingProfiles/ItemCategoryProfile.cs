using AutoMapper;
using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Dtos.Response;
using SmartStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.MappingProfiles
{
    public class ItemCategoryProfile : Profile
    {
        public ItemCategoryProfile()
        {
            CreateMap<ItemCategoryRequestDto, ItemCategory>()
                .ForMember(d => d.IsDeleted, obj => obj.MapFrom(src => false))
                .ForMember(d => d.IsActive, obj => obj.MapFrom(src => true)).ReverseMap();

            CreateMap<ItemCategory, ItemCategoryResponseDto>().ReverseMap();
        }
    }
}
