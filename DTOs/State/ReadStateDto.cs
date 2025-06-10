using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.DTOs.Community;

namespace HarvestCore.WebApi.DTOs.State
{
    public class ReadStateDto
    {
        public int IdState { get; set; }
        public int IdCountry { get; set; }
        public string StateCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int NumberOfCommunities { get; set; }
        // TODO: Implementar la lista de comunidades
        public List<ReadCommunityDto> Communities { get; set; } = new List<ReadCommunityDto>();
    }
}