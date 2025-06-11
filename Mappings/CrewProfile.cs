using AutoMapper;
using HarvestCore.WebApi.DTOs.Crew;
using HarvestCore.WebApi.DTOs.Community;
using HarvestCore.WebApi.DTOs.Harvester;
using HarvestCore.WebApi.Entites;
using System.Linq;

namespace HarvestCore.WebApi.Mappings
{
    public class CrewProfile : Profile
    {
        public CrewProfile()
        {
            // Mapeo de la entidad Crew a DTO de lectura
            CreateMap<Crew, ReadCrewDto>()
            // Regla para propiedad CommunityDetails
            // Indicamos a AutoMapper que mapee la propiedad Community de la entidad Crew
            // al DTO ReadCrewDto. Esto entrega la comunidad a la que pertenece el crew
                .ForMember(dest => dest.CommunityDetails, opt => opt.MapFrom(src => src.Community))
                // Regla para propiedad NumberOfHarvesters
                // Indicamos a AutoMapper que calcule el valor contando los elementos
                // de la colección Harvesters de la entidad Crew
                .ForMember(dest => dest.NumberOfHarvesters, opt => opt.MapFrom(src => src.Harvesters != null ? src.Harvesters.Count : 0 ))
                // Regla para propiedad Harvesters
                // Indicamos a AutoMapper que mapee la colección Harvesters de la entidad Crew
                // al DTO ReadCrewDto. Esto entrega la lista de cosechadores que pertenecen al crew
                .ForMember(dest => dest.Harvesters, opt => opt.MapFrom(src => src.Harvesters));

            // Mapeo de la entidad Crew a DTO de creación
            // No se necesitan reglas especiales ya que el mapeo es directo (i.e. todas las propiedades coinciden
            // entre el DTO y la entidad)
            CreateMap<Crew, CreateCrewDto>();

            // Mapeo de la entidad Crew a DTO de actualización
            // Indicamos a AutoMapper que mapee todos los miembros (propiedades) del DTO UpdateCrewDto
            // Esta condicion asegura que solo se mepeen los valores del DTO que no sean nulos.
            // Es util para operaciones de actualizacion parcial (i.e. PATCH)
            CreateMap<Crew, UpdateCrewDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
