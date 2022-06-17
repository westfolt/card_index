using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Models;
using card_index_BLL.Models.Dto;

namespace card_index_BLL.Interfaces
{
    public interface ICardService:ICrudService<TextCardDto>
    {
        Task<double> CalculateCardRatingAsync(int cardId);
        Task<IEnumerable<TextCardDto>> GetCardsForPeriodAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<TextCardDto>> GetCardsByFilterAsync(FilterModel filter);
        Task<IEnumerable<GenreDto>> GetAllGenresAsync();
        Task<int> AddGenreAsync(GenreDto model);
        Task UpdateGenreAsync(GenreDto model);
        Task DeleteGenreAsync(int modelId);
        Task AddRatingToCard(RateDetailDto model);
        Task DeleteRatingFromCard(int cardId, int userId);
    }
}
