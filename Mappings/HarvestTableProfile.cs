using AutoMapper;
using HarvestCore.WebApi.DTOs.HarvestTable;
using HarvestCore.WebApi.Entites;
using System.Linq;


namespace HarvestCore.WebApi.Mappings
{
    public class HarvestTableProfile : Profile
    {
        public HarvestTableProfile()
        {
            // Mapeo de la Entidad HarvestTable al DTO de Lectura
            CreateMap<HarvestTable, ReadHarvestTableDto>()
                .ForMember(dest => dest.NumberOfMacroTunnels, opt => opt.MapFrom(src => src.MacroTunnels != null ? src.MacroTunnels.Count : 0))
                .ForMember(dest => dest.MacroTunnels, opt => opt.MapFrom(src => src.MacroTunnels));

            // Mapeo del DTO de creación al DTO de creación
            CreateMap<CreateHarvestTabledto, HarvestTable>();

            // Mapeo del DTO de actualización al DTO de actualización
            CreateMap<UpdateHarvestTableDto, HarvestTable>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Mapeo de la Entidad HarvestTable a DTO de Actualización especificamente para PATCH (UpdateHarvestTableDto)
            CreateMap<HarvestTable, UpdateHarvestTableDto>();
        
        }
    }
}