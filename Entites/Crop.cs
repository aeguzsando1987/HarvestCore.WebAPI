using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client;
using HarvestCore.WebApi.Enums;

namespace HarvestCore.WebApi.Entites
{
    public class Crop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCrop { get; set; }

        [Required]
        [MaxLength(20)]
        public string CropKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;

        public string? Variety { get; set; }

        [Required]
        public CropCategory Category { get; set; }

        public CropSeasons? Season { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Harvest> Harvests { get; set; } = new List<Harvest>();

        
    }
}