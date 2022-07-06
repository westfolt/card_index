using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;
using card_index_DAL.Exceptions;
using card_index_DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using card_index_DAL.Entities.DataShapingModels;

namespace card_index_DAL.Repositories
{
    /// <summary>
    /// Repository to work with Text cards data set
    /// </summary>
    public class TextCardRepository : ITextCardRepository
    {
        private readonly CardIndexDbContext _db;
        /// <summary>
        /// Repository constructor, takes database context as parameter
        /// </summary>
        /// <param name="context">DB context</param>
        public TextCardRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        /// <summary>
        /// Takes all text cards, without connected instances
        /// </summary>
        /// <returns>Text cards collection</returns>
        public async Task<IEnumerable<TextCard>> GetAllAsync()
        {
            return await _db.TextCards.OrderBy(tc=>tc.Id).ToListAsync();
        }
        /// <summary>
        /// Takes all text cards, without connected instances,
        /// filtered by several parameters and paged
        /// </summary>
        /// <param name="filter">Filtering object</param>
        /// <returns>Text cards collection</returns>
        public async Task<IEnumerable<TextCard>> GetAllAsync(CardFilter filter)
        {
            return await _db.TextCards
                .Where(tc => filter.GenreId != 0 ? tc.GenreId == filter.GenreId : tc != null)
                .Where(tc => filter.AuthorId != 0 ? tc.Authors.Any(a => a.Id == filter.AuthorId) : tc != null)
                .Where(tc => tc.CardRating >= filter.Rating)
                .Where(tc => filter.CardName != "" ? tc.Title.Contains(filter.CardName) : tc != null)
                .OrderBy(tc=>tc.Id)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }
        /// <summary>
        /// Gets text card entity with given id from DB
        /// </summary>
        /// <param name="id">entity identifier to search</param>
        /// <returns>Object matching criteria</returns>
        public async Task<TextCard> GetByIdAsync(int id)
        {
            return await _db.TextCards.FirstOrDefaultAsync(tc => tc.Id == id);
        }
        /// <summary>
        /// Adds text card to DB
        /// </summary>
        /// <param name="entity">New text card to add</param>
        /// <returns>Async operation</returns>
        public async Task AddAsync(TextCard entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var alreadyExists = await _db.TextCards.FirstOrDefaultAsync(tc => tc.Id == entity.Id);
            if (alreadyExists == null)
            {
                _db.TextCards.Add(entity);
            }
            else
            {
                throw new EntityAlreadyExistsException($"TextCard with id: {entity.Id} already exists");
            }
        }
        /// <summary>
        /// Deletes given text card from DB
        /// </summary>
        /// <param name="entity">Text card to delete</param>
        public void Delete(TextCard entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var itemToDelete = _db.TextCards.FirstOrDefault(tc => tc.Id == entity.Id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"TextCard with id: {entity.Id} not found in db");

            _db.TextCards.Remove(itemToDelete);
        }
        /// <summary>
        /// Deletes text card with given id from DB
        /// </summary>
        /// <param name="id">Id of text card to delete</param>
        /// <returns>Async operation</returns>
        public async Task DeleteByIdAsync(int id)
        {
            var itemToDelete = await _db.TextCards.FirstOrDefaultAsync(tc => tc.Id == id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"TextCard with id: {id} not found in db)");

            _db.TextCards.Remove(itemToDelete);
        }
        /// <summary>
        /// Updates given text card in DB
        /// </summary>
        /// <param name="entity">Text card to update</param>
        public void Update(TextCard entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var existsInDb = _db.TextCards.Any(tc => tc.Id == entity.Id);

            if (!existsInDb)
                throw new EntityNotFoundException($"TextCard with id: {entity.Id} not found in db");

            _db.TextCards.Update(entity);
        }
        /// <summary>
        /// Gets total number of text cards,
        /// stored in database
        /// </summary>
        /// <returns>Number of objects</returns>
        public async Task<int> GetTotalNumberAsync()
        {
            return await _db.TextCards.CountAsync();
        }
        /// <summary>
        /// Gets total number of cards matching given filter
        /// </summary>
        /// <param name="filter">Filtering object</param>
        /// <returns>Number of cards</returns>
        public async Task<int> GetTotalNumberByFilterAsync(CardFilter filter)
        {
            return await _db.TextCards
                .Where(tc => filter.GenreId != 0 ? tc.GenreId == filter.GenreId : tc != null)
                .Where(tc => filter.AuthorId != 0 ? tc.Authors.Any(a => a.Id == filter.AuthorId) : tc != null)
                .Where(tc => tc.CardRating >= filter.Rating)
                .Where(tc => filter.CardName != "" ? tc.Title.Contains(filter.CardName) : tc != null)
                .CountAsync();
        }

        /// <summary>
        /// Takes all text cards, including other instances connected
        /// </summary>
        /// <returns>Text cards collection</returns>
        public async Task<IEnumerable<TextCard>> GetAllWithDetailsAsync()
        {
            return await _db.TextCards.Include(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .Include(tc => tc.Genre)
                .Include(tc => tc.Authors)
                .OrderBy(tc => tc.Id)
                .ToListAsync();
        }
        /// <summary>
        /// Takes all text cards, including other instances connected,
        /// filtered by several parameters
        /// </summary>
        /// <param name="filter">Filtering parameters</param>
        /// <returns>Text cards collection</returns>
        public async Task<IEnumerable<TextCard>> GetAllWithDetailsAsync(CardFilter filter)
        {
            return await _db.TextCards
                .Where(tc => filter.GenreId != 0 ? tc.GenreId == filter.GenreId : tc != null)
                .Where(tc => filter.AuthorId != 0 ? tc.Authors.Any(a => a.Id == filter.AuthorId) : tc != null)
                .Where(tc => tc.CardRating >= filter.Rating)
                .Where(tc => filter.CardName != "" ? tc.Title.Contains(filter.CardName) : tc != null)
                .OrderBy(tc => tc.Id)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Include(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .Include(tc => tc.Genre)
                .Include(tc => tc.Authors)
                .ToListAsync();
        }
        /// <summary>
        /// Gets text card with given id and all connected
        /// instances included from database
        /// </summary>
        /// <param name="id">Id of text card to search</param>
        /// <returns>Text cards collection</returns>
        public async Task<TextCard> GetByIdWithDetailsAsync(int id)
        {
            return await _db.TextCards.Include(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .Include(tc => tc.Genre)
                .Include(tc => tc.Authors)
                .OrderBy(tc => tc.Id)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }
    }
}
