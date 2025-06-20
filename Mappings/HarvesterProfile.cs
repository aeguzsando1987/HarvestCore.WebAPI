using AutoMapper;
using HarvestCore.WebApi.DTOs.Harvester;
using HarvestCore.WebApi.Entites;
using System;
using System.Linq;

namespace HarvestCore.WebApi.Mappings
{
    public class HarvesterProfile : Profile
    {
        public HarvesterProfile()
        {
            // Mapeo de la Entidad Harvester al DTO de Lectura (ReadHarvesterDto)
            CreateMap<Harvester, ReadHarvesterDto>()
                // Convierte el byte[] de Photo a una cadena Base64 si no es nulo.
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo != null ? Convert.ToBase64String(src.Photo) : null))
                // Convierte el byte[] de Encoder a una cadena Base64 si no es nulo.
                .ForMember(dest => dest.Encoder, opt => opt.MapFrom(src => src.Encoder != null ? Convert.ToBase64String(src.Encoder) : null))
                // Calcula NumberOfHarvests contando los registros en la colección Harvests.
                .ForMember(dest => dest.NumberOfHarvests, opt => opt.MapFrom(src => src.Harvests != null ? src.Harvests.Count : 0))
                // Mapea la colección de Harvests. Esto requiere un HarvestProfile.
                .ForMember(dest => dest.Harvests, opt => opt.MapFrom(src => src.Harvests))
                // Mapea la clave del crew desde la entidad CrewEntity
                .ForMember(dest => dest.Crew, opt => opt.MapFrom(src => 
                            src.CrewEntity != null ? src.CrewEntity.CrewKey : null))
                // Mapea los detalles completos del crew desde la entidad CrewEntity
                .ForMember(dest => dest.CrewDetails, opt => opt.MapFrom(src => src.CrewEntity));

            // Mapeo del DTO de Creación (CreateHarvesterDto) a la Entidad Harvester
            CreateMap<CreateHarvesterDto, Harvester>()
                // Convierte la cadena Base64 de Photo a byte[] si no es nula o vacía.
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Photo) ? Convert.FromBase64String(src.Photo) : null))
                // Convierte la cadena Base64 de Encoder a byte[] si no es nula o vacía.
                .ForMember(dest => dest.Encoder, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Encoder) ? Convert.FromBase64String(src.Encoder) : null));

            // Mapeo del DTO de Actualización (UpdateHarvesterDto) a la Entidad Harvester
            CreateMap<UpdateHarvesterDto, Harvester>()
                // Convierte la cadena Base64 de Photo a byte[] si no es nula o vacía.
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Photo) ? Convert.FromBase64String(src.Photo) : null))
                // Convierte la cadena Base64 de Encoder a byte[] si no es nula o vacía.
                .ForMember(dest => dest.Encoder, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Encoder) ? Convert.FromBase64String(src.Encoder) : null))
                // Condición para no actualizar con valores nulos, permitiendo PATCH.
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            // Mapeo de la Entidad Harvester a DTO de Actualización especificamente para PATCH (UpdateHarvesterDto)
            // Este mapeo es útil para permitir actualizaciones parciales en campos de entidad (PATCH).
            CreateMap<Harvester, UpdateHarvesterDto>()
                // Mapea la propiedad Photo del origen (src) al destino (dest)
                // Si la propiedad Photo del origen no es nula, convierte el array de bytes a una cadena Base64
                // La sintaxis opt => opt.MapFrom define una función que especifica cómo mapear esta propiedad
                // El operador ternario '?' evalúa si src.Photo es nulo y devuelve null si lo es, o la conversión si no lo es
                .ForMember(dest => dest.Photo, 
                            opt => opt.MapFrom(src => src.Photo != null ? 
                            Convert.ToBase64String(src.Photo) : null))
                // Mapea la propiedad Encoder del origen al destino usando la misma lógica
                // Convert.ToBase64String convierte el array de bytes a una representación de texto en formato Base64
                // que es adecuada para transmisión en JSON y almacenamiento en texto plano
                .ForMember(dest => dest.Encoder, 
                            opt => opt.MapFrom(src => src.Encoder != null ? 
                            Convert.ToBase64String(src.Encoder) : null));

        }
    }
}
