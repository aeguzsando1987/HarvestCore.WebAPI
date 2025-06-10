using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.DTOs.Crop
{
    public class ReadCropDto
    {
        public int IdCrop { get; set; }

        public string CropKey { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public string? Variety { get; set; }

        public string Category { get; set; } = string.Empty; // Enum como string

        public string? Season { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int NumberOfHarvests { get; set; }

        // public List<ReadHarvestDto> Harvests { get; set; } = new List<ReadHarvestDto>();
    }
}