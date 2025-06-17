using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.Data;
using AutoMapper;
using HarvestCore.WebApi.DTOs.State;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace HarvestCore.WebApi.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public StateRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReadStateDto> CreateStateAsync(CreateStateDto createStateDto)
        {
            var stateEntity = _mapper.Map<State>(createStateDto);
            _context.States.Add(stateEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReadStateDto>(stateEntity);            
        }

        public async Task<bool> DeleteStateAsync(int IdState)
        {
            // Buscar el estado en la base de datos por su ID
            var state = await _context.States.FindAsync(IdState);
            
            // Si el estado no existe, retornar falso
            if (state == null)
            {
                return false;
            }
            
            // Marcar el estado para ser eliminado
            _context.States.Remove(state);
            
            // Guardar los cambios en la base de datos y retornar verdadero si se eliminó al menos un registro
            // SaveChangesAsync() retorna el número de entidades afectadas, por lo que > 0 significa que al menos una entidad fue eliminada
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ReadStateDto>> GetAllStatesAsync()
        {
            return await _context.States
                .ProjectTo<ReadStateDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ReadStateDto?> GetStateByIdAsync(int id)
        {
            return await _context.States
                .Where(s => s.IdState == id)
                .ProjectTo<ReadStateDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ReadStateDto>> GetStateByCountryIdAsync(int idCountry)
        {
            return await _context.States
                .Where(s => s.IdCountry == idCountry)
                .ProjectTo<ReadStateDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> StateExistsAsync(int id)
        {
            return await _context.States.AnyAsync(s => s.IdState == id);
        }

        public async Task<ReadStateDto?> UpdateStateAsync(int id, UpdateStateDto updateStateDto)
        {
            var statetoUpdate = await _context.States.FindAsync(id);
            if (statetoUpdate == null)
            {
                return null;
            }
            _mapper.Map(updateStateDto, statetoUpdate);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReadStateDto>(statetoUpdate);
        }
    }
}