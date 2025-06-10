using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HarvestCore.WebApi.DTOs.Crop
{
    public class CreateCropDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string CropKey { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string ProductName { get; set; } = string.Empty;

        public string? Variety { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty; // Enum como string

        public string? Season { get; set; } // Enum como string, permite nulls  
    }
}