using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    public interface ICrudService<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(int id);
        Task<int> AddAsync(TModel model);
        Task UpdateAsync(TModel model);
        Task DeleteAsync(int modelId);
    }
}
