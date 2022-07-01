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
    }
}
