using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardIndexTests.WebApiTests.Helpers
{
    public class FakeGenreService : IGenreService
    {
        public Task<IEnumerable<GenreDto>> GetAllAsync()
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<GenreDto> GetByNameAsync(string name)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<int> AddAsync(GenreDto model)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task UpdateAsync(GenreDto model)
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

        public Task<IEnumerable<GenreDto>> GetAllAsync(PagingParametersModel parameters)
        {
            throw new CardIndexException("Something went wrong");
        }
    }
}
