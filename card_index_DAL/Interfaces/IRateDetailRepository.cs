using card_index_DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_DAL.Interfaces
{
    /// <summary>
    /// Repository to work with Rate details data set interface
    /// </summary>
    public interface IRateDetailRepository : IRepository<RateDetail>
    {
        /// <summary>
        /// Gets all rate details with connected instances included
        /// </summary>
        /// <returns>Rate details list</returns>
        Task<IEnumerable<RateDetail>> GetAllWithDetailsAsync();
    }
}
