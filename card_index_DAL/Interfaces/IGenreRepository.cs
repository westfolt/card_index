using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_DAL.Interfaces
{
    /// <summary>
    /// Repository to work with Genres data set interface
    /// </summary>
    public interface IGenreRepository : IRepository<Genre>
    {
        /// <summary>
        /// Takes all genres, including other instances connected,
        /// filtered by paging parameters
        /// </summary>
        /// <param name="parameters">Paging filter parameters</param>
        /// <returns>Genres collection</returns>
        Task<IEnumerable<Genre>> GetAllWithDetailsAsync(PagingParameters parameters);
        /// <summary>
        /// Takes all genres, without connected instances,
        /// filtered by paging parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>Genres collection</returns>
        Task<IEnumerable<Genre>> GetAllAsync(PagingParameters parameters);
        /// <summary>
        /// Gets genre with given id and all connected
        /// instances included from database
        /// </summary>
        /// <param name="id">Id of genre to search</param>
        /// <returns>Genre matching criteria</returns>
        Task<Genre> GetByIdWithDetailsAsync(int id);

        /// <summary>
        /// Gets genre with all connected entities matching given name
        /// </summary>
        /// <param name="name">Genre name to search</param>
        /// <returns>Genre object</returns>
        Task<Genre> GetByNameWithDetailsAsync(string name);
    }
}
