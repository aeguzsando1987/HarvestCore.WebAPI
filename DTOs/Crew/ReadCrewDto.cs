using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.DTOs.Community;
using HarvestCore.WebApi.DTOs.Harvester;

namespace HarvestCore.WebApi.DTOs.Crew
{
    public class ReadCrewDto
    {
        public int IdCrew { get; set; }

        public int IdCommunity { get; set; } 

        public string CrewKey { get; set; } = string.Empty;

        public string ResponsibleName { get; set; } = string.Empty;

        public string Community { get; set; } = string.Empty;

        public ReadCommunityDto CommunityDetails { get; set; } = null!; // Anidado para mostrar la comunidad a la que pertenece el crew

        public int NumberOfHarvesters { get; set; } // Cantidad de cosechadores que pertenecen al crew

        public List<ReadHarvesterDto> Harvesters { get; set; } = new List<ReadHarvesterDto>();
    }
}