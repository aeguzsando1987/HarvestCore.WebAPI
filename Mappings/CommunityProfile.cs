using AutoMapper;
using HarvestCore.WebApi.DTOs.Community;
using HarvestCore.WebApi.Entites;
using System.Linq;

namespace HarvestCore.WebApi.Mappings
{
    /// <summary>
    /// Perfil de mapeo para la entidad Community y sus DTOs relacionados.
    /// Esta clase configura AutoMapper para realizar conversiones entre la entidad Community
    /// y sus diferentes representaciones DTOs (Data Transfer Objects), facilitando
    /// la transferencia de datos entre las capas de la aplicación sin exponer
    /// directamente las entidades del dominio.
    /// 
    /// Para futuras implementaciones:
    /// - Agregar mapeos para nuevos DTOs que se creen para Community
    ///   Ejemplo: CreateMap<Community, CommunityStatsDto>()
    ///            .ForMember(dest => dest.TotalMembers, opt => opt.MapFrom(src => src.Crews.Sum(c => c.Members.Count)))
    /// - Mantener consistencia en las reglas de mapeo para propiedades relacionadas
    ///   Ejemplo: Usar siempre el mismo enfoque para mapear colecciones anidadas
    /// - Considerar agregar validaciones personalizadas si se requieren transformaciones complejas
    ///   Ejemplo: .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "Activa" : "Inactiva"))
    /// - Documentar cualquier regla de mapeo no trivial con comentarios explicativos
    /// </summary>
    public class CommunityProfile : Profile
    {
        public CommunityProfile()
        {
            // Mapeo de la entidad Community a DTO de lectura
            CreateMap<Community, ReadCommunityDto>()
            // Regla para propiedad NumberOfCrews
            // Indicamos a AutoMapper que calcule el valor contando los elementos
            // de la colección Crews de la entidad Community
                .ForMember(dest => dest.NumberOfCrews, opt => opt.MapFrom(src => src.Crews != null ? src.Crews.Count : 0))
            // Regla para propiedad Crews
            // Indicamos a AutoMapper que mapee la colección Crews de la entidad Community
            // al DTO ReadCommunityDto. Esto entrega la lista de crews
                .ForMember(dest => dest.Crews, opt => opt.MapFrom(src => src.Crews));

            // Mapeo de la entidad Community a DTO de creación
            // No se necesitan reglas especiales ya que el mapeo es directo (i.e. todas las propiedades coinciden
            // entre el DTO y la entidad)
            CreateMap<Community, CreateCommunityDto>();

            // Mapeo de la entidad Community a DTO de actualización
            CreateMap<Community, UpdateCommunityDto>()
                // Indicamos a AutoMapper que mapee todos los miembros (propiedades) del DTO UpdateCommunityDto
                // Esta condicion asegura que solo se mepeen los valores del DTO que no sean nulos.
                // Es util para operaciones de actualizacion parcial (i.e. PATCH)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            // Mapeo de la Entidad Community a DTO de Actualización especificamente para PATCH (UpdateCommunityDto)
            // Este mapeo es útil para permitir actualizaciones parciales en campos de entidad (PATCH).
            CreateMap<Community, UpdateCommunityDto>();
        }
    }
        
}