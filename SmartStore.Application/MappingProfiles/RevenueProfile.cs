using AutoMapper;
using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.MappingProfiles
{
    public class RevenueProfile : Profile
    {
        public RevenueProfile()
        {
            CreateMap<RevenueRequestDto, Revenue>()
                 .ForMember(d => d.IsDeleted, obj => obj.MapFrom(src => false))
                 .ForMember(d => d.IsActive, obj => obj.MapFrom(src => true)).ReverseMap();
        }
    }
}
