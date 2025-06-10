using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HarvestCore.WebApi.DTOs.HarvestTable
{
    public class UpdateHarvestTableDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string HarvestTableKey { get; set; } = string.Empty;
    }
}