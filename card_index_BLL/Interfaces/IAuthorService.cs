using card_index_BLL.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    /// <summary>
    /// Logic to work with author repository
    /// </summary>
    public interface IAuthorService : ICrudService<AuthorDto>
    {
        /// <summary>
        /// Gets authors for given time period
        /// </summary>
        /// <param name="startYear">Start year to search</param>
        /// <param name="endYear">End year to search</param>
        /// <returns>Authors list matching given years range</returns>
        Task<IEnumerable<AuthorDto>> GetAuthorsForPeriodAsync(int startYear, int endYear);
    }
}
