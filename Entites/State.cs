using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarvestCore.WebApi.Entites
{
    public class State
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdState { get; set; }

        [Required]
        public int IdCountry { get; set; }

        [Required]
        [MaxLength(3)]
        public string StateCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [ForeignKey("IdCountry")]
        public Country Country { get; set; } = null!;

        public ICollection<Community> Communities { get; set; } = new List<Community>(); 

    }
}