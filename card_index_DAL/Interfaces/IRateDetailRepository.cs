using card_index_DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_DAL.Interfaces
{
    public interface IRateDetailRepository : IRepository<RateDetail>
    {
        Task<IEnumerable<RateDetail>> GetAllWithDetailsAsync();
    }
}
