using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;
using System.Collections.Generic;
using System.Threading.Tasks;

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
