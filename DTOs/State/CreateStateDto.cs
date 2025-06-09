using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.DTOs.State
{
    public class CreateStateDto
    {
        [Required]
        public int IdCountry { get; set; }

        [Required]
        [StringLength(3,MinimumLength = 1)]
        public string StateCode { get; set; } = string.Empty;

        [Required]
        [StringLength(50,MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
    }
}