using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Models.Dto;
using card_index_DAL.Entities;

namespace card_index_BLL.Interfaces
{
    public interface IAuthorService : ICrudService<AuthorDto>
    {
        Task<IEnumerable<AuthorDto>> GetAuthorsForPeriodAsync(int startYear, int endYear);
    }
}
