using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.DTOs.Harvester
{
    public class ReadHarvesterDto
    {
        public int IdHarvester { get; set; }

        public string HarvesterKey { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int IdCrew { get; set; }

        public byte[]? Photo { get; set; }

        public byte[]? Encoder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int NumberOfHarvesters { get; set; }

        // TODO: Implementar la navegación 1:N Harvester:Harvest
        //public List<ReadHarvestDto> Harvests { get; set; } = new List<ReadHarvestDto>();
    }
}