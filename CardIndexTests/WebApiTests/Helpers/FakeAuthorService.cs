using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;

namespace CardIndexTests.WebApiTests.Helpers
{
    public class FakeAuthorService:IAuthorService
    {
        public Task<IEnumerable<AuthorDto>> GetAllAsync()
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<AuthorDto> GetByIdAsync(int id)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<int> AddAsync(AuthorDto model)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task UpdateAsync(AuthorDto model)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task DeleteAsync(int modelId)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<int> GetTotalNumberAsync()
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<IEnumerable<AuthorDto>> GetAuthorsForPeriodAsync(int startYear, int endYear)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<IEnumerable<AuthorDto>> GetAllAsync(PagingParametersModel parameters)
        {
            throw new CardIndexException("Something went wrong");
        }
    }
}
