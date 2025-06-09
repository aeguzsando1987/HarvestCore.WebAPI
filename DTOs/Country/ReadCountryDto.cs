using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.DTOs.State;
namespace HarvestCore.WebApi.DTOs.Country
{
    public class ReadCountryDto
    {
        public int IdCountry { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int NumberOfStates { get; set; }
        // TODO: Implementar la lista de estados
        public List<ReadStateDto> States { get; set; } = new List<ReadStateDto>();
    }
}