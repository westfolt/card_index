using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_DAL.Interfaces
{
    /// <summary>
    /// Repository to work with Text cards data set interface
    /// </summary>
    public interface ITextCardRepository : IRepository<TextCard>
    {
        /// <summary>
        /// Takes all text cards, including other instances connected
        /// </summary>
        /// <returns>Text cards collection</returns>
        Task<IEnumerable<TextCard>> GetAllWithDetailsAsync();
        /// <summary>
        /// Takes all text cards, including other instances connected,
        /// filtered by paging parameters
        /// </summary>
        /// <param name="parameters">Paging filter parameters</param>
        /// <returns>Text cards collection</returns>
        Task<IEnumerable<TextCard>> GetAllWithDetailsAsync(PagingParameters parameters);
        /// <summary>
        /// Gets text card with given id and all connected
        /// instances included from database
        /// </summary>
        /// <param name="id">Id of text card to search</param>
        /// <returns>Text cards collection</returns>
        Task<TextCard> GetByIdWithDetailsAsync(int id);
        /// <summary>
        /// Takes all text cards, without connected instances,
        /// filtered by paging parameters
        /// </summary>
        /// <param name="parameters">Paging filter object</param>
        /// <returns>Text cards collection</returns>
        Task<IEnumerable<TextCard>> GetAllAsync(PagingParameters parameters);
    }
}
