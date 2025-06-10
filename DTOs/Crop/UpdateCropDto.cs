using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HarvestCore.WebApi.DTOs.Crop
{
    public class UpdateCropDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string? CropKey { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string? ProductName { get; set; }

        [StringLength(100)]
        public string? Variety { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        public string? Season { get; set; }
    }
}