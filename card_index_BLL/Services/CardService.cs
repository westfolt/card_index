using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models;
using card_index_BLL.Models.Dto;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;

namespace card_index_BLL.Services
{
    public class CardService : ICardService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CardService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

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

        public async Task<int> AddAsync(TextCardDto model)
        {
            try
            {
                var mapped = _mapper.Map<TextCardDto, TextCard>(model);
                await _unitOfWork.TextCardRepository.AddAsync(mapped);
                await _unitOfWork.SaveChangesAsync();
                return mapped.Id;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot add text card to db", ex);
            }
        }

        public async Task UpdateAsync(TextCardDto model)
        {
            try
            {
                var mapped = _mapper.Map<TextCardDto, TextCard>(model);
                _unitOfWork.TextCardRepository.Update(mapped);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot update text card with id: {model.Id}", ex);
            }
        }

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

        public async Task AddRatingToCard(RateDetailDto model)
        {
            var alreadyExists = (await _unitOfWork.RateDetailRepository.GetAllAsync())
                .FirstOrDefault(rd => rd.TextCardId == model.TextCardId && rd.UserId == model.UserId);
            if (alreadyExists != null)
                throw new CardIndexException($"User has already left his rating");

            try
            {
                var mapped = _mapper.Map<RateDetailDto, RateDetail>(model);
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
