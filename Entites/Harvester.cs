using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarvestCore.WebApi.Entites
{
    public class Harvester
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdHarvester { get; set; }

        [Required]
        [MaxLength(20)]
        public string HarvesterKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int IdCrew { get; set; }

        public byte[]? Photo { get; set; }

        public byte[]? Encoder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("IdCrew")]
        public Crew CrewEntity { get; set; } = null!;

        // Navegación 1:N Harvester:Harvest - Un Harvester puede tener muchos registros en Harvests
        public ICollection<Harvest> Harvests { get; set; } = new List<Harvest>();

        
    }
}