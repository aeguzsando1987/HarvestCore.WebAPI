using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HarvestCore.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HarvestCore.WebApi.Repositories
{
    /// <summary>
    /// Un repositorio es un patrón de diseño que separa la lógica de acceso a datos
    /// de la lógica de negocio, encapsulando la interacción con la base de datos.
    /// Permite realizar operaciones CRUD de forma genérica para cualquier entidad,
    /// facilitando la reutilización de código y el mantenimiento de la aplicación.
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            // Validacion de argumentos. _context no puede ser null, sino arroja una excepcion
            _context = context ?? throw new ArgumentNullException(nameof(context)); 
            _dbSet = _context.Set<T>();        
        }

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="id">El ID de la entidad a buscar.</param>
        /// <returns>La entidad encontrada o null si no se encuentra.</returns>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Obtiene todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Obtiene una lista de entidades que cumplen con el predicado especificado.
        /// </summary>
        /// <param name="predicate">El predicado que define las condiciones de búsqueda.</param>
        /// <returns>Una lista de entidades que cumplen con el predicado.</returns>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Agrega una entidad al repositorio.
        /// </summary>
        /// <param name="entity">La entidad a agregar.</param>
        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Agrega una colección de entidades al repositorio.
        /// </summary>
        /// <param name="entities">La colección de entidades a agregar.</param>

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Actualiza una entidad en el repositorio.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity); // Asocia la entidad al contexto
            _context.Entry(entity).State = EntityState.Modified; // Marca la entidad como modificada
        }

        /// <summary>
        /// Elimina una entidad del repositorio.
        /// </summary>
        /// <param name="entity">La entidad a eliminar.</param>
        public virtual void Remove(T entity)
        {
            if(_context.Entry(entity).State == EntityState.Detached) // Si la entidad no esta en el contexto
            {
                _dbSet.Attach(entity); // Asocia la entidad al contexto
            }
            _dbSet.Remove(entity); // Marca la entidad como eliminada
        }

        /// <summary>
        /// Elimina una colección de entidades del repositorio.
        /// </summary>
        /// <param name="entities">La colección de entidades a eliminar.</param>
        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
        
        /// <summary>
        /// Obtiene el conteo de entidades en el repositorio.
        /// </summary>
        /// <returns>El conteo de entidades en el repositorio.</returns>
        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        /// <summary>
        /// Verifica si existe una entidad que cumpla con el predicado especificado.
        /// </summary>
        /// <param name="predicate">El predicado que define las condiciones de búsqueda.</param>
        /// <returns>Verdadero si existe al menos una entidad que cumpla con el predicado, de lo contrario falso.</returns>
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// Guarda los cambios realizados en el repositorio.
        /// </summary>
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}