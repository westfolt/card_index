using card_index_DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_DAL.Interfaces
{
    public interface ITextCardRepository : IRepository<TextCard>
    {
        Task<IEnumerable<TextCard>> GetAllWithDetailsAsync();
        Task<TextCard> GetByIdWithDetailsAsync(int id);
    }
}
