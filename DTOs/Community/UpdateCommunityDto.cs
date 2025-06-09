using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HarvestCore.WebApi.DTOs.Community
{
    public class UpdateCommunityDto
    {
        [Required]
        public int IdState { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 1)]
        public string CommunityKey { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string CityName { get; set; } = string.Empty;
    }
}