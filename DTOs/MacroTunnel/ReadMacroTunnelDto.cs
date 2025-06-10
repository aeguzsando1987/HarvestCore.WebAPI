using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.DTOs.MacroTunnel
{
    public class ReadMacroTunnelDto
    {
        public int IdMacroTunnel { get; set; }

        public string MacroTunnelKey { get; set; } = string.Empty;

        public int IdHarvestTable { get; set; }

        public string HarvestTableKey { get; set; } = string.Empty; // Valor clave de la tabla de cosecha incluido directamente en este DTO para evitar joins adicionales en consultas

        public int? WalkwayNumber { get; set; }

        public int NumberOfHarvests { get; set; }

        // TODO: Agregar la relacion con HarvestTable y Harvests
        // public ReadHarvestTableDto? HarvestTable { get; set; }
        // public List<ReadHarvestDto> Harvests { get; set; } = new List<ReadHarvestDto>();

    }
}