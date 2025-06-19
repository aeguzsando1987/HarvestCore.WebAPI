using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.Data;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using HarvestCore.WebApi.DTOs.Crew;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HarvestCore.WebApi.Repositories
{
    public class CrewRepository : ICrewRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CrewRepository(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

        public async Task<IEnumerable<ReadCrewDto>> GetAllCrewsAsync()
        {
            // Obtenemos todas las cuadrillas (crews) de la base de datos
            return await _context.Crews
                // Incluimos la entidad Community relacionada para acceder a sus propiedades
                .Include(c => c.CommunityEntity)
                // Incluimos la colección de Harvesters asociados a cada cuadrilla
                .Include(c => c.Harvesters)
                // Proyectamos los resultados al DTO usando AutoMapper para transformar entidades a DTOs
                .ProjectTo<ReadCrewDto>(_mapper.ConfigurationProvider)
                // Convertimos los resultados a una lista y ejecutamos la consulta de forma asíncrona
                .ToListAsync();
        }

        public async Task<ReadCrewDto?> GetCrewByIdAsync(int id)
        {
            var crewEntity = await _context.Crews
                .Include(c => c.CommunityEntity)
                .Include(c => c.Harvesters)
                .FirstOrDefaultAsync(C => C.IdCrew == id);

            return _mapper.Map<ReadCrewDto>(crewEntity);

        }

        public async Task<ReadCrewDto?> GetCrewByKeyAsync(string key)
        {
            var crewEntity = await _context.Crews
                .Include(c => c.CommunityEntity)
                .Include(c => c.Harvesters)
                .FirstOrDefaultAsync(c => c.CrewKey == key);

            return _mapper.Map<ReadCrewDto>(crewEntity);
        }

        public async Task<ReadCrewDto> CreateCrewAsync(CreateCrewDto crewDto)
        {
            var crewEntity = _mapper.Map<Crew>(crewDto);
            _context.Crews.Add(crewEntity);
            await _context.SaveChangesAsync();

            var createdCrew = await GetCrewByIdAsync(crewEntity.IdCrew);
            return createdCrew!;
        }

        public async Task<ReadCrewDto?> UpdateCrewAsync(int id, UpdateCrewDto crewDto)
        {
            // Buscamos la cuadrilla por su ID en la base de datos
            var crewEntity = await _context.Crews.FindAsync(id);
            
            // Si no encontramos la cuadrilla, retornamos null para indicar que no existe
            if (crewEntity == null)
            {
                return null;
            }
            
            // Utilizamos AutoMapper para actualizar las propiedades de la entidad existente
            // con los valores del DTO, manteniendo el mismo objeto en memoria
            _mapper.Map(crewDto, crewEntity);
            
            // Marcamos la entidad como modificada para que Entity Framework
            // la actualice en la base de datos durante el SaveChanges
            _context.Entry(crewEntity).State = EntityState.Modified;
            
            // Guardamos los cambios en la base de datos de forma asíncrona
            await _context.SaveChangesAsync();

            // Retornamos la cuadrilla actualizada, utilizando el método existente
            // que incluye todas las relaciones necesarias para el DTO completo
            return await GetCrewByIdAsync(id);
        }

        public async Task<bool> DeleteCrewAsync(int id)
        {
            var crewEntity = await _context.Crews.FindAsync(id);
            if (crewEntity == null)
            {
                return false;
            }

            _context.Crews.Remove(crewEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CrewExistsAsync(int id)
        {
            return await _context.Crews.AnyAsync(c => c.IdCrew == id);
        }

        public async Task<Crew?> GetCrewEntityByIdAsync(int id)
        {
            return await _context.Crews
                .FirstOrDefaultAsync(c => c.IdCrew == id);
        }


    }
}

