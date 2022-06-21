using card_index_BLL.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    public interface IAuthorService : ICrudService<AuthorDto>
    {
        Task<IEnumerable<AuthorDto>> GetAuthorsForPeriodAsync(int startYear, int endYear);
    }
}
