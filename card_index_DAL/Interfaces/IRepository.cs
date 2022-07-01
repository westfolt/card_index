using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_DAL.Interfaces
{
    /// <summary>
    /// Generic repository interface,
    /// holds basic CRUD operations
    /// </summary>
    /// <typeparam name="TEntity">Type placeholder, must be class</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets all instances of given type from DB
        /// </summary>
        /// <returns>Objects collection of given type</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();
        /// <summary>
        /// Gets entity with given id and type from DB
        /// </summary>
        /// <param name="id">entity identifier to search</param>
        /// <returns>Object matching criteria</returns>
        Task<TEntity> GetByIdAsync(int id);
        /// <summary>
        /// Adds entity to DB
        /// </summary>
        /// <param name="entity">New object to add</param>
        /// <returns>Async operation</returns>
        Task AddAsync(TEntity entity);
        /// <summary>
        /// Deletes given entity from DB
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        void Delete(TEntity entity);
        /// <summary>
        /// Deletes entity with given id and type from DB
        /// </summary>
        /// <param name="id">Id of entity to delete</param>
        /// <returns>Async operation</returns>
        Task DeleteByIdAsync(int id);
        /// <summary>
        /// Updates given entity in DB
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(TEntity entity);
        /// <summary>
        /// Gets total number of entities of given type,
        /// stored in database
        /// </summary>
        /// <returns>Number of objects</returns>
        Task<int> GetTotalNumberAsync();
    }
}
