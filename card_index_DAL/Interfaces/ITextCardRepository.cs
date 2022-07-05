using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;
using System.Collections.Generic;
using System.Threading.Tasks;
using card_index_DAL.Entities.DataShapingModels;

namespace card_index_DAL.Interfaces
{
    /// <summary>
    /// Repository to work with Text cards data set interface
    /// </summary>
    public interface ITextCardRepository : IRepository<TextCard>
    {
        /// <summary>
        /// Gets total number of cards matching given filter
        /// </summary>
        /// <param name="filter">Filtering object</param>
        /// <returns>Number of cards</returns>
        Task<int> GetTotalNumberByFilterAsync(CardFilter filter);
        /// <summary>
        /// Takes all text cards, including other instances connected
        /// </summary>
        /// <returns>Text cards collection</returns>
        Task<IEnumerable<TextCard>> GetAllWithDetailsAsync();
        /// <summary>
        /// Takes all text cards, including other instances connected,
        /// filtered by several parameters
        /// </summary>
        /// <param name="parameters">Filtering parameters</param>
        /// <returns>Text cards collection</returns>
        Task<IEnumerable<TextCard>> GetAllWithDetailsAsync(CardFilter filter);
        /// <summary>
        /// Gets text card with given id and all connected
        /// instances included from database
        /// </summary>
        /// <param name="id">Id of text card to search</param>
        /// <returns>Text cards collection</returns>
        Task<TextCard> GetByIdWithDetailsAsync(int id);
        /// <summary>
        /// Takes all text cards, without connected instances,
        /// filtered by several parameters and paged
        /// </summary>
        /// <param name="parameters">Filtering object</param>
        /// <returns>Text cards collection</returns>
        Task<IEnumerable<TextCard>> GetAllAsync(CardFilter filter);
    }
}
