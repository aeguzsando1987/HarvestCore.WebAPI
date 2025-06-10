using Microsoft.AspNetCore.Mvc;
using HarvestCore.WebApi.Repositories;
using HarvestCore.WebApi.DTOs.Country;
using HarvestCore.WebApi.Entites;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace HarvestCore.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase{
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CountryController> _logger;

        public CountryController(ICountryRepository countryRepository, ILogger<CountryController> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        // GET: api/Country
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadCountryDto>>> GetCountries()
        {
            // Registra un mensaje informativo en el log del sistema
            _logger.LogInformation("Obteniendo lista de paises");
            
            // Llama al método GetAllAsync del repositorio para obtener todos los países de la base de datos
            // ICountryRepository hereda de IGenericRepository<Country> que define este método
            var countries = await _countryRepository.GetAllAsync();
            
            // Convierte la colección de entidades Country a DTOs de tipo ReadCountryDto
            // Usa LINQ Select para transformar cada entidad Country en un objeto ReadCountryDto
            var countryDtos = countries.Select(c => new ReadCountryDto
            {
                // Mapea las propiedades de la entidad Country a las propiedades del DTO
                IdCountry = c.IdCountry,     // El ID único del país
                Name = c.Name,               // El nombre del país
                CountryCode = c.CountryCode, // El código ISO del país
                
                // Calcula el número de estados asociados al país
                // c.States es una colección de navegación de tipo ICollection<State> en la entidad Country
                // ?. es el operador de navegación segura que evita NullReferenceException si States es null
                // ?? es el operador de coalescencia nula que devuelve 0 si el resultado anterior es null
                NumberOfStates = c.States?.Count ?? 0,
            }).ToList();
            
            // Devuelve un código de estado HTTP 200 (OK) con la colección de DTOs
            // ActionResult<T> permite devolver tanto tipos de resultado HTTP como objetos tipados
            return Ok(countryDtos);
        }
    }
}
