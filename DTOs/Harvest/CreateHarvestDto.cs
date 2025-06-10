using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HarvestCore.WebApi.DTOs.Harvest
{
    public class CreateHarvestDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string HarvestKey { get; set; } = string.Empty;

        [Required]
        public int IdHarvester { get; set; }

        [Required]
        public int IdMacroTunnel { get; set; }

        public int? IdCrop { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El peso debe ser mayor a 0")]
        public float Weight { get; set; }

        [Required]
        public string QualityLevel { get; set; } = string.Empty; // Enum como string

        [Required]
        public DateTime TransDate { get; set; }

        public string? Photo { get; set; } // string codificado en base64
    }
}