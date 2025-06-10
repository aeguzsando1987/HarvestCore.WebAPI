using AutoMapper;
using HarvestCore.WebApi.DTOs.State;
using HarvestCore.WebApi.Entites;
using System.Linq;

namespace HarvestCore.WebApi.Mappings
{
    public class StateProfile : Profile
    {

        public StateProfile()
        {
            // Mapeo de entidad State a DTO de lectura 
            CreateMap<State, ReadStateDto>()
                // Regla para propiedad NumberOfCommunities
                // Indicamos a AutoMapper que calcule el valor contando los elementos
                // de la colección Communities de la entidad State
                .ForMember(dest => dest.NumberOfCommunities, opt => opt.MapFrom(src => src.Communities != null ? src.Communities.Count : 0))
                // Regla para propiedad Communities
                // Indicamos a AutoMapper que mapee la colección Communities de la entidad State
                // al DTO ReadStateDto. Esto entrega la lista de comunidades
                .ForMember(dest => dest.Communities, opt => opt.MapFrom(src => src.Communities));
            
            // Mapeo de DTO de creación a entidad State
            // No se necesitan reglas especiales ya que el mapeo es directo (i.e. todas las propiedades coinciden
            // entre el DTO y la entidad)
            CreateMap<UpdateStateDto, State>();

            // Mapeo de DTO de actualización a entidad State
            CreateMap<State, UpdateStateDto>()
                // Indicamos a AutoMapper que mapee todos los miembros (propiedades) del DTO UpdateStateDto
                // Esta condicion asegura que solo se mepeen los valores del DTO que no sean nulos.
                // Es util para operaciones de actualizacion parcial (i.e. PATCH)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}