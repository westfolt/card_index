using System.Collections.Generic;
using System.Threading.Tasks;
using card_index_DAL.Entities.DataShaping;

namespace card_index_DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
        Task DeleteByIdAsync(int id);
        void Update(TEntity entity);
        Task<int> GetTotalNumberAsync();
    }
}
