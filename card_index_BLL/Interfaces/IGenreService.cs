using card_index_BLL.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDto>> GetAllAsync();
        Task<GenreDto> GetByNameAsync(string name);
        Task<int> AddAsync(GenreDto model);
        Task UpdateAsync(GenreDto model);
        Task DeleteAsync(int modelId);
    }
}
