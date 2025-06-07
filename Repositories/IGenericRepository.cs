using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


namespace HarvestCore.WebApi.Repositories
{
    /// <summary>
    /// Interfaz genérica para el repositorio de entidades que define operaciones CRUD básicas.
    /// Permite abstraer la implementación concreta del acceso a datos, facilitando
    /// la reutilización de código y el cambio de tecnologías de persistencia sin afectar
    /// a la lógica de negocio que depende de esta interfaz.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que será gestionada por el repositorio</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Busca una entidad por su ID de forma asíncrona
        /// </summary>
        /// <param name="id">Identificador único de la entidad</param>
        /// <returns>La entidad encontrada o null si no existe</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las entidades de forma asíncrona
        /// </summary>
        /// <returns>Colección de todas las entidades</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Busca entidades que cumplan con una condición específica
        /// </summary>
        /// <param name="predicate">Expresión lambda que define la condición de búsqueda</param>
        /// <returns>Colección de entidades que cumplen la condición</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Agrega una nueva entidad de forma asíncrona
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Agrega múltiples entidades de forma asíncrona
        /// </summary>
        /// <param name="entities">Colección de entidades a agregar</param>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Actualiza una entidad existente
        /// </summary>
        /// <param name="entity">Entidad con los cambios a aplicar</param>
        void Update(T entity);

        /// <summary>
        /// Elimina una entidad de forma no asíncrona, pues EF Core sigue los cambios
        /// </summary>
        /// <param name="entity">Entidad a eliminar</param>
        void Remove(T entity);

        /// <summary>
        /// Elimina múltiples entidades de forma no asíncrona, pues EF Core sigue los cambios
        /// </summary>
        /// <param name="entities">Colección de entidades a eliminar</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Cuenta el número total de entidades de forma asíncrona
        /// </summary>
        /// <returns>Número total de entidades</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Verifica si existe alguna entidad que cumpla con una condición específica
        /// </summary>
        /// <param name="predicate">Expresión lambda que define la condición de búsqueda</param>
        /// <returns>True si existe al menos una entidad que cumpla la condición, False en caso contrario</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Guarda todos los cambios realizados en el repositorio de forma asíncrona
        /// </summary>
        Task SaveAsync();
       
    }
}