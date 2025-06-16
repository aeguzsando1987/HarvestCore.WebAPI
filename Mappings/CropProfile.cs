using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.DTOs.Crop;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.Enums;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace HarvestCore.WebApi.Mappings
{
    public class CropProfile : Profile
    {
        public CropProfile()
        {
            // Mapeo de entidad Crop DTO de lectura
            CreateMap<Crop, ReadCropDto>()
                // Convertir enum Category a representación de string
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
                // Convertir enum Season a representación de string
                .ForMember(dest => dest.Season, opt => opt.MapFrom(src => src.Season.ToString()))
                // Calcular NumberOfHarvests
                .ForMember(dest => dest.NumberOfHarvests, opt => opt.MapFrom(src => src.Harvests != null ? src.Harvests.Count : 0));

            // Mapeo de creación
            CreateMap<CreateCropDto, Crop>()
                // Convierte la cadena Category al enum Category de la entidad Crop
                // La cedena debe ser un valor valido en el enum
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<CropCategory>(src.Category, true)))
                // Convierte el string Season al enum CropSeasons de la entidad Crop
                // Realiza una validación para manejar valores nulos o vacíos
                // Si el valor es válido, utiliza Enum.Parse para la conversión
                // Si es nulo o vacío, asigna null al campo Season del destino
                .ForMember(dest => dest.Season, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Season) ? 
                                                Enum.Parse<CropSeasons>(src.Season, true): (CropSeasons?)null))
                // Asegurar que fechas de creación y modificación se manejen correctamente
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapeo de actualización
            CreateMap<UpdateCropDto, Crop>()
                // Convierte la cadena Category de nuevo al enum CropCategory
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<CropCategory>(src.Category, true)))
                // Convierte la cadena Season de nuevo al enum CropSeasons
                .ForMember(dest => dest.Season, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Season) ?
                                                Enum.Parse<CropSeasons>(src.Season, true) : (CropSeasons?)null))
                // Actualiza la fecha de modificación
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                // Condición para no actualizar con valores nulos (excepto los mapeados explicitamente arriba)
                // Para propiedades como CropKey, ProductName, Variety, si son null, en el DTO no se actualizaran en la entidad
                .ForAllMembers( opts =>
                {
                    // Excluye las propiedades que ya tienen mapeos personalizados para evitar duplicidad
                    if (opts.DestinationMember.Name == nameof(Crop.Category) ||
                        opts.DestinationMember.Name == nameof(Crop.Season) ||
                        opts.DestinationMember.Name == nameof(Crop.UpdatedAt))
                    {
                        // Salta la configuración para estos miembros ya que tienen mapeos explícitos arriba
                        return;
                    }
                    // Para el resto de propiedades, aplica la condición de no actualizar con valores nulos
                    // Esto permite actualizaciones parciales (PATCH) donde solo se modifican los campos proporcionados
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });
            
            // Mapeo de la Entidad Crop a DTO de Actualización especificamente para PATCH (UpdateCropDto)
            // Este mapeo es útil para permitir actualizaciones parciales en campos de entidad (PATCH).
            CreateMap<Crop, UpdateCropDto>();
        }
    }
}