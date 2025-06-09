using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.DTOs.Country
{
    public class UpdateCountryDto
    {
        [Required]
        [MaxLength(2)]
        public string CountryCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}