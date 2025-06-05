using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarvestCore.WebApi.Entites
{
    public class HarvestTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdHarvestTable { get; set; }

        [Required]
        [MaxLength(20)]
        public string HarvestTableKey { get; set; } = string.Empty;

        // Navegación 1:N HarvestTable:MacroTunnel - Un HarvestTable puede tener muchos registros en MacroTunnels
        public ICollection<MacroTunnel> MacroTunnels { get; set; } = new List<MacroTunnel>();
    }
}