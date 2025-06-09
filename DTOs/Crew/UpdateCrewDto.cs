using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HarvestCore.WebApi.DTOs.Crew
{
    public class UpdateCrewDto
    {
        [Required]
        public int IdCommunity { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 1)]
        public string CrewKey { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string ResponsibleName { get; set; } = string.Empty;
    }
}