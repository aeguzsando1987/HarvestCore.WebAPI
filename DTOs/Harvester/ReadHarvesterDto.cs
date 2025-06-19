using System;
using System.Collections.Generic;
using HarvestCore.WebApi.DTOs.Harvest;
using HarvestCore.WebApi.DTOs.Crew;

namespace HarvestCore.WebApi.DTOs.Harvester
{
    public class ReadHarvesterDto
    {
        public int IdHarvester { get; set; }
        public string HarvesterKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int IdCrew { get; set; }
        public string Crew { get; set; } = string.Empty;
        public string? Photo { get; set; } // Para Base64
        public string? Encoder { get; set; } // Para Base64
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int NumberOfHarvests { get; set; } // Conteo de la colección Harvests
        public ReadCrewDto CrewDetails { get; set; } = null!; // Anidado para mostrar la comunidad a la que pertenece el crew

        // Relacion con la entidad Harvest. Permitira obtener la lista de cosechas asociadas al cosechador
        public List<ReadHarvestDto> Harvests { get; set; } = new List<ReadHarvestDto>();
    }
}