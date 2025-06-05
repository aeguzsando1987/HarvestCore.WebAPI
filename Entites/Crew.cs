using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarvestCore.WebApi.Entites
{
    public class Crew
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCrew { get; set; }

        [Required]
        public int IdCommunity { get; set; }

        [Required]
        [MaxLength(3)]
        public string CrewKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ResponsibleName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Community { get; set; } = string.Empty; // Duplicado para rapidez de búsqueda; mantenido en sincronización con Community.Name

        [ForeignKey("IdCommunity")]
        public Community CommunityEntity { get; set; } = null!;

        public ICollection<Harvester> Harvesters { get; set; } = new List<Harvester>();
    }
}