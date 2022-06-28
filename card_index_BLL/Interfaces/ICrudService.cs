using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    /// <summary>
    /// Basic interface with crud operations
    /// </summary>
    /// <typeparam name="TModel">Class to perform crud operations wih, one of Dto's</typeparam>
    public interface ICrudService<TModel> where TModel : class
    {
        /// <summary>
        /// Gets all instances from Db
        /// </summary>
        /// <returns>Models list</returns>
        Task<IEnumerable<TModel>> GetAllAsync();
        /// <summary>
        /// Gets instance with given id
        /// </summary>
        /// <param name="id">id to search</param>
        /// <returns>Model with id</returns>
        Task<TModel> GetByIdAsync(int id);
        /// <summary>
        /// Adds new instance to db
        /// </summary>
        /// <param name="model">model to add</param>
        /// <returns>Id of added model</returns>
        Task<int> AddAsync(TModel model);
        /// <summary>
        /// Updates existing model in db
        /// </summary>
        /// <param name="model">model with data for update</param>
        /// <returns>Async operation</returns>
        Task UpdateAsync(TModel model);
        /// <summary>
        /// Deletes model with given id from db
        /// </summary>
        /// <param name="modelId">Id of instance to delete</param>
        /// <returns>Async operation</returns>
        Task DeleteAsync(int modelId);
    }
}
