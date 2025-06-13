using System.ComponentModel.DataAnnotations;

namespace HarvestCore.WebApi.DTOs.Harvester
{
    public class CreateHarvesterDto
    {
        [Required]
        [StringLength(20)]
        public string HarvesterKey { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int IdCrew { get; set; }

        // La foto y el encoder se reciben como string Base64 y son opcionales
        public string? Photo { get; set; }

        public string? Encoder { get; set; }
    }
}
