using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.DTOs.Country;
using HarvestCore.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HarvestCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CountryController> _logger;

        public CountryController(ICountryRepository countryRepository, ILogger<CountryController> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }
    }
}