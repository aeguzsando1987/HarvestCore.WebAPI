using AutoMapper;
using HarvestCore.WebApi.Data;
using HarvestCore.WebApi.DTOs.Country;
using HarvestCore.WebApi.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CountryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReadCountryDto?> GetCountryByIdAsync(int id)
        {
            var countryEntity = await _context.Countries
                                                .Include(c => c.States) // Incluir States para el mapeo
                                                .FirstOrDefaultAsync(c => c.IdCountry == id);
            return _mapper.Map<ReadCountryDto>(countryEntity);
        }

        public async Task<IEnumerable<ReadCountryDto>> GetAllCountriesAsync()
        {
            var countryEntities = await _context.Countries
                                                .Include(c => c.States) // Incluir States
                                                .ToListAsync();
            return _mapper.Map<IEnumerable<ReadCountryDto>>(countryEntities);
        }

        public async Task<ReadCountryDto> CreateCountryAsync(CreateCountryDto createCountryDto)
        {
            var countryEntity = _mapper.Map<Country>(createCountryDto);
            _context.Countries.Add(countryEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReadCountryDto>(countryEntity); // Mapea la entidad (con ID) a ReadCountryDto
        }

        public async Task<ReadCountryDto?> UpdateCountryAsync(int id, UpdateCountryDto updateCountryDto)
        {
            var countryEntity = await _context.Countries.FindAsync(id);
            if (countryEntity == null)
            {
                return null; // O lanzar una excepción NotFound
            }

            // Mapea los valores del DTO a la entidad existente
            _mapper.Map(updateCountryDto, countryEntity);
            
            _context.Countries.Update(countryEntity); // O _context.Entry(countryEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            // Mapea la entidad actualizada de nuevo a ReadCountryDto para devolverla (incluyendo States si es necesario)
            // Es posible que necesitemos recargar la entidad para obtener las States si no se gestionan en el mapeo de Update
            var updatedEntity = await _context.Countries
                                              .Include(c => c.States)
                                              .FirstOrDefaultAsync(c => c.IdCountry == id);
            return _mapper.Map<ReadCountryDto>(updatedEntity);
        }

        public async Task<bool> DeleteCountryAsync(int id)
        {
            var countryEntity = await _context.Countries.FindAsync(id);
            if (countryEntity == null)
            {
                return false; // O lanzar una excepción NotFound
            }
            _context.Countries.Remove(countryEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CountryExistsAsync(int id)
        {
            return await _context.Countries.AnyAsync(c => c.IdCountry == id);
        }

        public async Task<Country?> GetCountryEntityByIdAsync(int id)
        {
            return await _context.Countries.FindAsync(id);
        }
    }
}