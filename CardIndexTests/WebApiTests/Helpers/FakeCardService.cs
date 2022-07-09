using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardIndexTests.WebApiTests.Helpers
{
    public class FakeCardService : ICardService
    {
        public Task<IEnumerable<TextCardDto>> GetAllAsync()
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<TextCardDto> GetByIdAsync(int id)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<int> AddAsync(TextCardDto model)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task UpdateAsync(TextCardDto model)
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

        public Task<double> CalculateCardRatingAsync(int cardId)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<IEnumerable<TextCardDto>> GetCardsForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task AddRatingToCard(RateDetailDto model)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task DeleteRatingFromCard(int cardId, int userId)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<RateDetailDto> GetRateDetailByUserIdCardId(int userId, int textCardId)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<IEnumerable<TextCardDto>> GetAllAsync(CardFilterParametersModel cardFilterParameters)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<int> GetTotalNumberByFilterAsync(CardFilterParametersModel cardFilterParameters)
        {
            throw new CardIndexException("Something went wrong");
        }
    }
}
