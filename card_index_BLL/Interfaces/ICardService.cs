using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    /// <summary>
    /// Logic to work with text card repository
    /// </summary>
    public interface ICardService : ICrudService<TextCardDto>
    {
        /// <summary>
        /// Calculates card rating, based on rateDetails array
        /// </summary>
        /// <param name="cardId">Id of card to calculate</param>
        /// <returns>New rate value</returns>
        Task<double> CalculateCardRatingAsync(int cardId);
        /// <summary>
        /// Gets cards for given date range
        /// </summary>
        /// <param name="startDate">start date of range</param>
        /// <param name="endDate">end date of range</param>
        /// <returns>cards list, corresponding to given period</returns>
        Task<IEnumerable<TextCardDto>> GetCardsForPeriodAsync(DateTime startDate, DateTime endDate);
        /// <summary>
        /// Adds new rating to rate details and recalculates rate value for card
        /// </summary>
        /// <param name="model">new rate detail, connecting card and user</param>
        /// <returns>Async operation</returns>
        Task AddRatingToCard(RateDetailDto model);
        /// <summary>
        /// Deletes rating given by user and recalculates card rate
        /// </summary>
        /// <param name="cardId">card id to delete user rating</param>
        /// <param name="userId">user id, who gave rating</param>
        /// <returns>Async operation</returns>
        Task DeleteRatingFromCard(int cardId, int userId);
       /// <summary>
       /// Gets rate detail for given card and user
       /// </summary>
       /// <param name="userId">id of user, who has given rating</param>
       /// <param name="textCardId">id of text card</param>
       /// <returns>Rate detail matching criteria, if exists</returns>
        Task<RateDetailDto> GetRateDetailByUserIdCardId(int userId, int textCardId);
        /// <summary>
        /// Gets all cards with filtering and paging
        /// </summary>
        /// <param name="cardFilterParameters">Filtering parameters model</param>
        /// <returns>Cards list</returns>
        Task<IEnumerable<TextCardDto>> GetAllAsync(CardFilterParametersModel cardFilterParameters);

        /// <summary>
        /// Gets total number of cards in storage matching filter
        /// </summary>
        /// <returns>Cards number</returns>
        public Task<int> GetTotalNumberByFilter(CardFilterParametersModel cardFilterParameters);
    }
}
