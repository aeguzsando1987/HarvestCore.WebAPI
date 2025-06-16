using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.DTOs.Country;
using HarvestCore.WebApi.Entites;

using AutoMapper;

namespace HarvestCore.WebApi.Mappings
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, ReadCountryDto>()
                .ForMember(dest => dest.NumberOfStates, opt => opt.MapFrom(src => src.States != null ? src.States.Count : 0))
                .ForMember(dest => dest.States, opt => opt.MapFrom(src => src.States));
            CreateMap<CreateCountryDto, Country>();
            CreateMap<UpdateCountryDto, Country>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null));
            CreateMap<Country, UpdateCountryDto>();
        }
    }
}