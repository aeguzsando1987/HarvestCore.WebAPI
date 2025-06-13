using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace HarvestCore.WebApi.DTOs.Harvester
{
    public class UpdateHarvesterDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        public int? IdCrew { get; set; }

        // La foto y el encoder se reciben como string Base64 y son opcionales
        public string? Photo { get; set; }

        public string? Encoder { get; set; }
    }
}