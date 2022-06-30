using System.Collections.Generic;
using System.Threading.Tasks;
using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;

namespace card_index_DAL.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<IEnumerable<Genre>> GetAllWithDetailsAsync(PagingParameters parameters);
        Task<IEnumerable<Genre>> GetAllAsync(PagingParameters parameters);
    }
}
