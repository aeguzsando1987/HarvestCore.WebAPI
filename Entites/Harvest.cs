using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HarvestCore.WebApi.Enums;

namespace HarvestCore.WebApi.Entites
{
    public class Harvest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdHarvest { get; set; }

        [Required]
        [MaxLength(20)]
        public string HarvestKey { get; set; } = string.Empty;

        [Required]
        public int IdHarvester { get; set; }

        [Required]
        public int IdMacroTunnel { get; set; }

        public int? IdCrop { get; set; }

        [Required]
        public float Weight { get; set; }

        [Required]
        public QualityLevels QualityLevel { get; set; }

        [Required]
        public DateTime TransDate { get; set; } = DateTime.UtcNow;

        public byte[]? Photo { get; set; }

        [ForeignKey("IdHarvester")]
        public Harvester HarvesterEntity { get; set; } = null!;

        [ForeignKey("IdMacroTunnel")]
        public MacroTunnel MacroTunnelEntity { get; set; } = null!;

        [ForeignKey("IdCrop")]
        public Crop CropEntity { get; set; } = null!;

    }
}