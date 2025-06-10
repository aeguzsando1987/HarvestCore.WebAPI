using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HarvestCore.WebApi.DTOs.MacroTunnel
{
    public class CreateMacroTunnelDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string MacroTunnelKey { get; set; } = string.Empty;

        [Required]
        public int IdHarvestTable { get; set; }

        public int? WalkwayNumber { get; set; }
        
    }
}