using card_index_BLL.Models;
using card_index_BLL.Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    public interface ICardService : ICrudService<TextCardDto>
    {
        Task<double> CalculateCardRatingAsync(int cardId);
        Task<IEnumerable<TextCardDto>> GetCardsForPeriodAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<TextCardDto>> GetCardsByFilterAsync(FilterModel filter);
        Task AddRatingToCard(RateDetailDto model);
        Task DeleteRatingFromCard(int cardId, int userId);
    }
}
