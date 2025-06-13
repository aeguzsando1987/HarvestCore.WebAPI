using AutoMapper;
using HarvestCore.WebApi.DTOs.Harvest;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.Enums;
using Microsoft.Data.SqlClient;
using System;

namespace HarvestCore.WebApi.Mappings
{
    public class HarvestProfile : Profile
    {
        public HarvestProfile()
        {
            // Mapeo de la entidad Harvest al DTO de lectura
            // Este mapeo convierte la entidad Harvest en un objeto ReadHarvestDto para exposición a la API
            CreateMap<Harvest, ReadHarvestDto>()
                // Obtiene la clave del cosechador desde la entidad relacionada HarvesterEntity
                .ForMember(dest => dest.HarvesterKey, opt => opt.MapFrom(src => src.HarvesterEntity.HarvesterKey))
                // Obtiene la clave del macrotúnel desde la entidad relacionada MacroTunnelEntity
                .ForMember(dest => dest.MacroTunnelKey, opt => opt.MapFrom(src => src.MacroTunnelEntity.MacroTunnelKey)) 
                // Obtiene la clave del cultivo desde la entidad relacionada CropEntity
                .ForMember(dest => dest.CropKey, opt => opt.MapFrom(src => src.CropEntity.CropKey))
                // Obtiene el nombre del producto del cultivo desde la entidad relacionada CropEntity
                .ForMember(dest => dest.CropProductName, opt => opt.MapFrom(src => src.CropEntity.ProductName))
                // Convierte el enum QualityLevel a una representación de cadena para mejor legibilidad
                .ForMember(dest => dest.QualityLevel, opt => opt.MapFrom(src => src.QualityLevel.ToString()))
                // Convierte el array de bytes de la foto a una cadena Base64 para transmisión web
                // Si la foto es nula, asigna null al DTO para evitar errores
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo != null ? Convert.ToBase64String(src.Photo) : null));

            // Mapeo del DTO de creación a la entidad Harvest
            // Este mapeo permite convertir los datos enviados por el cliente en una entidad Harvest para su persistencia
            CreateMap<CreateHarvestDto, Harvest>()
                // Convierte la cadena QualityLevel recibida en el enum correspondiente en la entidad
                // El parámetro true permite una comparación sin distinción entre mayúsculas y minúsculas
                .ForMember(dest => dest.QualityLevel, opt => opt.MapFrom(src => Enum.Parse<QualityLevels>(src.QualityLevel, true)))
                // Convierte la cadena Base64 de la foto a un array de bytes para almacenamiento en la base de datos
                // Si la foto es nula, asigna null para evitar errores
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo != null ? Convert.FromBase64String(src.Photo) : null))
                // Excluye la propiedad Photo de las validaciones automáticas ya que se maneja de forma especial
                .ForSourceMember(src => src.Photo, opt => opt.DoNotValidate());

            // Mapeo del DTO de actualización a la entidad Harvest
            // Este mapeo permite actualizar una entidad Harvest existente con datos parciales (PATCH)
            CreateMap<UpdateHarvestDto, Harvest>()
                // Convierte la cadena QualityLevel en el enum correspondiente en la entidad
                .ForMember(dest => dest.QualityLevel, opt => opt.MapFrom(src => Enum.Parse<QualityLevels>(src.QualityLevel, true)))
                // Convierte la cadena Base64 de la foto a un array de bytes
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo != null ? Convert.FromBase64String(src.Photo) : null))
                // Excluye la propiedad Photo de las validaciones automáticas
                .ForSourceMember(src => src.Photo, opt => opt.DoNotValidate())
                // Configura reglas especiales para todas las propiedades del mapeo
                .ForAllMembers(opts =>
                    {
                        // Si la propiedad es una de las listadas, no aplica la condición de nulidad
                        // Esto permite que estas propiedades específicas se actualicen siempre
                        if (opts.DestinationMember.Name == nameof(Harvest.QualityLevel) ||
                            opts.DestinationMember.Name == nameof(Harvest.Photo) ||
                            opts.DestinationMember.Name == nameof(Harvest.IdHarvester) ||
                            opts.DestinationMember.Name == nameof(Harvest.IdMacroTunnel) ||
                            opts.DestinationMember.Name == nameof(Harvest.Weight) ||
                            opts.DestinationMember.Name == nameof(Harvest.TransDate)
                        )
                        {
                            return;
                        }
                        // Para el resto de propiedades, solo se actualizan si el valor no es nulo
                        // Esto permite actualizaciones parciales (PATCH) donde solo se modifican los campos proporcionados
                        opts.Condition((src, dest, srcMember) => srcMember != null);
                    }
                );

            
        }
    } 
}