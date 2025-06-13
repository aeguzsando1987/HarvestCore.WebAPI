    using AutoMapper;
using HarvestCore.WebApi.DTOs.MacroTunnel;
using HarvestCore.WebApi.Entites;
using System.Linq;

namespace HarvestCore.WebApi.Mappings
{
    public class MacroTunnelProfile : Profile
    {
        public MacroTunnelProfile()
        {
            // Mapeo de la Entidad MacroTunnel al DTO de Lectura (ReadMacroTunnelDto)
            CreateMap<MacroTunnel, ReadMacroTunnelDto>()
                // Mapeamos HarvestTableKey desde la propiedad de navegación para evitar joins manuales.
                .ForMember(dest => dest.HarvestTableKey, opt => opt.MapFrom(src => src.HarvestTable.HarvestTableKey))
                // Calculamos NumberOfHarvests contando los registros en la colección Harvests.
                .ForMember(dest => dest.NumberOfHarvests, opt => opt.MapFrom(src => src.Harvests != null ? src.Harvests.Count : 0));

            // Mapeo del DTO de Creación (CreateMacroTunnelDto) a la Entidad MacroTunnel
            // Mapeo directo, ya que las propiedades coinciden.
            CreateMap<CreateMacroTunnelDto, MacroTunnel>();

            // Mapeo del DTO de Actualización (UpdateMacroTunnelDto) a la Entidad MacroTunnel
            CreateMap<UpdateMacroTunnelDto, MacroTunnel>()
                // Condición para no actualizar con valores nulos, permitiendo PATCH.
                // Esto funciona mejor si las propiedades en UpdateMacroTunnelDto no son requeridas.
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
