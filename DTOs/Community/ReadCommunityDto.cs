using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.DTOs.Crew;

namespace HarvestCore.WebApi.DTOs.Community
{
    public class ReadCommunityDto
    {
        public int IdCommunity { get; set; } 
        public int IdState { get; set; }
        public string StateName { get; set; } = string.Empty; 
        public string CommunityKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public int NumberOfCrews { get; set; } 
        public List<ReadCrewDto> Crews { get; set; } = new List<ReadCrewDto>();
    }
}