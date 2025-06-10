using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.DTOs.Harvest
{
    public class ReadHarvestDto
    {
        public int IdHarvest { get; set; }

        public string HarvestKey { get; set; } = string.Empty;

        public int IdHarvester { get; set; }
        public string HarvesterKey { get; set; } = string.Empty;

        public int IdMacroTunnel { get; set; }
        public string MacroTunnelKey { get; set; } = string.Empty;

        public int? IdCrop { get; set; }
        public string? CropKey { get; set; } = string.Empty;
        public string? CropProductName { get; set; }

        public float Weight { get; set; }

        public string QualityLevel { get; set; } = string.Empty;

        public DateTime TransDate { get; set; }

        public string? Photo { get; set; }
    }
}