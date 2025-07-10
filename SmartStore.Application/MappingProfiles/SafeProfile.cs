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
    public class SafeProfile : Profile
    {
        public SafeProfile()
        {
            CreateMap<SafeRequestDto, Safe>()
             .ForMember(d => d.IsDeleted, obj => obj.MapFrom(src => false))
             .ForMember(d => d.IsActive, obj => obj.MapFrom(src => true)).ReverseMap();

            CreateMap<Safe, SafeResponseDto>().ReverseMap();
        }
    }
}
