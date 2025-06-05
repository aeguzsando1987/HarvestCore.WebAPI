using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Runtime.CompilerServices;

namespace HarvestCore.WebApi.Entites
{
    public class Community
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCommunity { get; set; }

        [Required]
        public int IdState { get; set; }

        [Required]
        [MaxLength(3)]
        public string CommunityKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string CityName { get; set; } = string.Empty;

        [ForeignKey("IdState")]
        public State State { get; set; } = null!;

        public ICollection<Crew> Crews { get; set; } = new List<Crew>();

    }
}