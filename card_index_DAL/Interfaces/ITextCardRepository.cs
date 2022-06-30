using card_index_DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using card_index_DAL.Entities.DataShaping;

namespace card_index_DAL.Interfaces
{
    public interface ITextCardRepository : IRepository<TextCard>
    {
        Task<IEnumerable<TextCard>> GetAllWithDetailsAsync();
        Task<IEnumerable<TextCard>> GetAllWithDetailsAsync(PagingParameters parameters);
        Task<TextCard> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<TextCard>> GetAllAsync(PagingParameters parameters);
    }
}
