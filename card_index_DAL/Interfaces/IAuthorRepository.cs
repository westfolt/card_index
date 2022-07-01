using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_DAL.Interfaces
{
    /// <summary>
    /// Repository to work with Authors data set interface
    /// </summary>
    public interface IAuthorRepository : IRepository<Author>
    {
        /// <summary>
        /// Gets all author entities with connected objects
        /// included from DB
        /// </summary>
        /// <returns>Author entities collection</returns>
        Task<IEnumerable<Author>> GetAllWithDetailsAsync();
        /// <summary>
        /// Gets author with given id from DB,
        /// includes all connected entities details
        /// </summary>
        /// <param name="id">Author id to search</param>
        /// <returns>Author entity, matching criteria</returns>
        Task<Author> GetByIdWithDetailsAsync(int id);
        /// <summary>
        /// Gets all author entities with connected objects
        /// included from DB, matching given paging filter
        /// </summary>
        /// <returns>Author entities collection</returns>
        Task<IEnumerable<Author>> GetAllWithDetailsAsync(PagingParameters parameters);
        /// <summary>
        /// Gets all author entities without connected objects
        /// included from DB
        /// </summary>
        /// <returns>Author entities collection</returns>
        Task<IEnumerable<Author>> GetAllAsync(PagingParameters parameters);
    }
}
