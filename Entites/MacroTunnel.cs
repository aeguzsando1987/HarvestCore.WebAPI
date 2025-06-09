using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarvestCore.WebApi.Entites
{
    public class MacroTunnel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMacroTunnel { get; set; }

        [Required]
        [MaxLength(20)]
        public string MacroTunnelKey { get; set; } = string.Empty;

        [Required]
        public int IdHarvestTable { get; set; }

        public int? WalkwayNumber { get; set; }

        [ForeignKey("IdHarvestTable")]
        public HarvestTable HarvestTable { get; set; } = null!;

        // Navegación 1:N MacroTunnel:Harvest - Un MacroTunnel puede tener muchos registros en Harvests
        public ICollection<Harvest> Harvests { get; set; } = new List<Harvest>();
    }
}