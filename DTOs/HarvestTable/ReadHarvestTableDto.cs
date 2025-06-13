using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.DTOs.MacroTunnel;

namespace HarvestCore.WebApi.DTOs.HarvestTable
{
    public class ReadHarvestTableDto
    {
        public int IdHarvestTable { get; set; }
        public string HarvestTableKey { get; set; } = string.Empty;
        public int NumberOfMacroTunnels { get; set; }
        public List<ReadMacroTunnelDto> MacroTunnels { get; set; } = new List<ReadMacroTunnelDto>();
    }
}