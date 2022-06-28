using AutoMapper;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models;
using card_index_BLL.Models.Dto;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_BLL.Services
{
    /// <summary>
    /// Implements interface between webapi and repository
    /// </summary>
    public class CardService : ICardService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Takes mapper for mapping entities and object to work wid DAL
        /// </summary>
        /// <param name="mapper">Maps cards from DB to Dto</param>
        /// <param name="unitOfWork">Object for work with DAL</param>
        public CardService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets all cards from db
        /// </summary>
        /// <returns>Cards list</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<IEnumerable<TextCardDto>> GetAllAsync()
        {
            try
            {
                var takenFromDb = await _unitOfWork.TextCardRepository.GetAllWithDetailsAsync();
                var mapped = _mapper.Map<IEnumerable<TextCard>, IEnumerable<TextCardDto>>(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot get text cards", ex);
            }
        }

        /// <summary>
        /// Gets card with given id from db
        /// </summary>
        /// <param name="id">Id of card to search</param>
        /// <returns>Card with given id, if exists</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<TextCardDto> GetByIdAsync(int id)
        {
            try
            {
                var takenFromDb = await _unitOfWork.TextCardRepository.GetByIdWithDetailsAsync(id);
                var mapped = _mapper.Map<TextCard, TextCardDto>(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get text card with id: {id}", ex);
            }
        }

        /// <summary>
        /// Adds new card to db
        /// </summary>
        /// <param name="model">Card model to add</param>
        /// <returns>Id of added card</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<int> AddAsync(TextCardDto model)
        {
            try
            {
                var mapped = _mapper.Map<TextCardDto, TextCard>(model);
                var genreExists =
                    (await _unitOfWork.GenreRepository.GetAllAsync()).FirstOrDefault(g => g.Title == model.GenreName);
                mapped.Id = 0;
                if (genreExists != null)
                {
                    mapped.Genre = genreExists;
                    mapped.GenreId = genreExists.Id;
                    genreExists.TextCards.Add(mapped);
                    _unitOfWork.GenreRepository.Update(genreExists);
                }
                await _unitOfWork.TextCardRepository.AddAsync(mapped);
                await _unitOfWork.SaveChangesAsync();
                return mapped.Id;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot add text card to db", ex);
            }
        }

        /// <summary>
        /// Updates existing card
        /// </summary>
        /// <param name="model">Card model with values for update</param>
        /// <returns>Async operation</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task UpdateAsync(TextCardDto model)
        {
            try
            {
                var mapped = _mapper.Map<TextCardDto, TextCard>(model);
                var genreExists =
                    (await _unitOfWork.GenreRepository.GetAllAsync()).FirstOrDefault(g => g.Title == model.GenreName);
                if (genreExists != null)
                {
                    mapped.Genre = genreExists;
                    mapped.GenreId = genreExists.Id;
                    genreExists.TextCards.Add(mapped);
                    _unitOfWork.GenreRepository.Update(genreExists);
                }

                _unitOfWork.TextCardRepository.Update(mapped);
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot update text card with id: {model.Id}", ex);
            }
        }

        /// <summary>
        /// Deletes existing card from DB
        /// </summary>
        /// <param name="modelId">Card id to delete</param>
        /// <returns>Async operation</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task DeleteAsync(int modelId)
        {
            try
            {
                await _unitOfWork.TextCardRepository.DeleteByIdAsync(modelId);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot delete text card with id: {modelId}", ex);
            }
        }

        /// <summary>
        /// Calculates card rating, based on rateDetails array
        /// </summary>
        /// <param name="cardId">Id of card to calculate</param>
        /// <returns>New rate value</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<double> CalculateCardRatingAsync(int cardId)
        {
            try
            {
                return (await _unitOfWork.RateDetailRepository.GetAllAsync())
                    .Where(rd => rd.TextCardId == cardId).Average(rd => rd.RateValue);
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot calculate rating for text card with id: {cardId}", ex);
            }
        }

        /// <summary>
        /// Gets cards for given date range
        /// </summary>
        /// <param name="startDate">start date of range</param>
        /// <param name="endDate">end date of range</param>
        /// <returns>cards list, corresponding to given period</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<IEnumerable<TextCardDto>> GetCardsForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var takenFromDb = (await _unitOfWork.TextCardRepository.GetAllWithDetailsAsync())
                    .Where(tc => tc.ReleaseDate >= startDate && tc.ReleaseDate <= endDate);
                return _mapper.Map<IEnumerable<TextCard>, IEnumerable<TextCardDto>>(takenFromDb);
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot get text cards for desired date range", ex);
            }
        }

        /// <summary>
        /// Gets cards, matching given filter
        /// </summary>
        /// <param name="filter">Filter model for card search</param>
        /// <returns>Cards list, matching desired parameters</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<IEnumerable<TextCardDto>> GetCardsByFilterAsync(FilterModel filter)
        {
            if (filter == null)
                throw new CardIndexException("Search filter is null");

            var takenFromDb = await _unitOfWork.TextCardRepository.GetAllWithDetailsAsync();
            if (filter.AuthorId != null)
                takenFromDb = takenFromDb.Where(tc => tc.Authors.Any(a => a.Id == filter.AuthorId));

            if (filter.GenreId != null)
                takenFromDb = takenFromDb.Where(tc => tc.GenreId == filter.GenreId);

            if (filter.Rating != null)
                takenFromDb = takenFromDb.Where(tc => tc.CardRating >= filter.Rating);

            var mapped = _mapper.Map<IEnumerable<TextCard>, IEnumerable<TextCardDto>>(takenFromDb);
            return mapped;
        }

        /// <summary>
        /// Adds new rating to rate details and recalculates rate value for card
        /// </summary>
        /// <param name="model">new rate detail, connecting card and user</param>
        /// <returns>Async operation</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task AddRatingToCard(RateDetailDto model)
        {
            var alreadyExists = (await _unitOfWork.RateDetailRepository.GetAllAsync())
                .FirstOrDefault(rd => rd.TextCardId == model.TextCardId && rd.UserId == model.UserId);
            if (alreadyExists != null)
                throw new CardIndexException($"User has already left his rating");

            try
            {
                var mapped = _mapper.Map<RateDetailDto, RateDetail>(model);
                mapped.Id = 0;
                await _unitOfWork.RateDetailRepository.AddAsync(mapped);
                await _unitOfWork.SaveChangesAsync();
                double newRating = await CalculateCardRatingAsync(model.TextCardId);
                var cardToUpdate = await _unitOfWork.TextCardRepository.GetByIdWithDetailsAsync(model.TextCardId);
                cardToUpdate.CardRating = newRating;
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot add rating for cardId {model.TextCardId} from userId {model.UserId}", ex);
            }
        }

        /// <summary>
        /// Deletes rating given by user and recalculates card rate
        /// </summary>
        /// <param name="cardId">card id to delete user rating</param>
        /// <param name="userId">user id, who gave rating</param>
        /// <returns>Async operation</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task DeleteRatingFromCard(int cardId, int userId)
        {
            var alreadyExists = (await _unitOfWork.RateDetailRepository.GetAllAsync())
                .FirstOrDefault(rd => rd.TextCardId == cardId && rd.UserId == userId);

            if (alreadyExists == null)
                throw new CardIndexException($"User has not left his rating, nothing to delete");

            try
            {
                _unitOfWork.RateDetailRepository.Delete(alreadyExists);
                await _unitOfWork.SaveChangesAsync();
                double newRating = await CalculateCardRatingAsync(cardId);
                var cardToUpdate = await _unitOfWork.TextCardRepository.GetByIdWithDetailsAsync(cardId);
                cardToUpdate.CardRating = newRating;
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot add rating for cardId {cardId} from userId {userId}", ex);
            }
        }
    }
}
