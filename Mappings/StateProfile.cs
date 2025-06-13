using AutoMapper;
using HarvestCore.WebApi.DTOs.State; // Asegúrate que CreateStateDto está aquí
using HarvestCore.WebApi.DTOs.Community; // Para ReadCommunityDto en ReadStateDto
using HarvestCore.WebApi.Entites;
using System.Linq;

namespace HarvestCore.WebApi.Mappings
{
    public class StateProfile : Profile
    {
        public StateProfile()
        {
            // Mapeo de entidad State a DTO de lectura (ReadStateDto)
            CreateMap<State, ReadStateDto>()
                .ForMember(dest => dest.NumberOfCommunities, opt => opt.MapFrom(src => src.Communities != null ? src.Communities.Count : 0))
                // Asegúrate que ReadStateDto tiene la propiedad 'Communities' de tipo List<ReadCommunityDto>
                // y que existe un CommunityProfile para mapear Community a ReadCommunityDto.
                .ForMember(dest => dest.Communities, opt => opt.MapFrom(src => src.Communities));

            // Mapeo del DTO de Creación (CreateStateDto) a la Entidad State
            CreateMap<CreateStateDto, State>();

            // Mapeo del DTO de Actualización (UpdateStateDto) a la Entidad State
            CreateMap<UpdateStateDto, State>()
                // La condición ForAllMembers es útil si algunas propiedades del DTO fueran opcionales
                // para permitir actualizaciones parciales (PATCH).
                // Dado que todas las propiedades en UpdateStateDto son requeridas,
                // este mapeo funcionará como un reemplazo completo (PUT).
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}