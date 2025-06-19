using AutoMapper;
using HarvestCore.WebApi.DTOs.Crew;
using HarvestCore.WebApi.Entites;
using System.Linq;

namespace HarvestCore.WebApi.Mappings
{
    public class CrewProfile : Profile
    {
        public CrewProfile()
        {
            // Mapeo de la entidad Crew a DTO de lectura (ReadCrewDto)
            CreateMap<Crew, ReadCrewDto>()
                // Obtiene el nombre de la comunidad desde la entidad relacionada CommunityEntity
                .ForMember(dest => dest.Community, opt => opt.MapFrom(src => 
                    src.CommunityEntity != null ? src.CommunityEntity.Name : null))
                // Mapea los detalles completos de la comunidad desde la entidad relacionada CommunityEntity
                .ForMember(dest => dest.CommunityDetails, opt => opt.MapFrom(src => src.CommunityEntity))
                // Calcula el número de cosechadores contando los elementos en la colección Harvesters
                .ForMember(dest => dest.NumberOfHarvesters, opt => opt.MapFrom(src => 
                    src.Harvesters != null ? src.Harvesters.Count : 0))
                // Mapea la colección completa de cosechadores al DTO
                .ForMember(dest => dest.Harvesters, opt => opt.MapFrom(src => src.Harvesters));

            // Mapeo del DTO de creación (CreateCrewDto) a la entidad Crew
            CreateMap<CreateCrewDto, Crew>();

            // Mapeo del DTO de actualización (UpdateCrewDto) a la entidad Crew (para PUT)
            CreateMap<UpdateCrewDto, Crew>();

            // Mapeo de la Entidad Crew a DTO de Actualización (UpdateCrewDto) para PATCH
            CreateMap<Crew, UpdateCrewDto>();
        }
    }
}
