using card_index_DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using card_index_DAL.Entities.DataShaping;

namespace card_index_DAL.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<IEnumerable<Author>> GetAllWithDetailsAsync();
        Task<Author> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Author>> GetAllWithDetailsAsync(PagingParameters parameters);
        Task<IEnumerable<Author>> GetAllAsync(PagingParameters parameters);
    }
}
