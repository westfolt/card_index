using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    /// <summary>
    /// Logic to work with genre repository
    /// </summary>
    public interface IGenreService
    {
        /// <summary>
        /// Gets all genres from DB
        /// </summary>
        /// <returns>Genre list mapped to Dto</returns>
        Task<IEnumerable<GenreDto>> GetAllAsync();
        /// <summary>
        /// Gets genre with given name
        /// </summary>
        /// <param name="name">Genre name to search</param>
        /// <returns>Genre matching criteria</returns>
        Task<GenreDto> GetByNameAsync(string name);
        /// <summary>
        /// Adds new genre to DB
        /// </summary>
        /// <param name="model">New genre to add</param>
        /// <returns>Id of added genre</returns>
        Task<int> AddAsync(GenreDto model);
        /// <summary>
        /// Updates genre in DB
        /// </summary>
        /// <param name="model">Model, containing data for update</param>
        /// <returns>Async operation</returns>
        Task UpdateAsync(GenreDto model);
        /// <summary>
        /// Deletes genre from DB
        /// </summary>
        /// <param name="modelId">Id of genre to delete</param>
        /// <returns>Async operation</returns>
        Task DeleteAsync(int modelId);
        /// <summary>
        /// Gets total number of genres stored
        /// </summary>
        /// <returns>Genre number</returns>
        public Task<int> GetTotalNumber();
        /// <summary>
        /// Gets all genres with paging
        /// </summary>
        /// <param name="parameters">Paging model parameters</param>
        /// <returns>Genres list</returns>
        Task<IEnumerable<GenreDto>> GetAllAsync(PagingParametersModel parameters);
    }
}
