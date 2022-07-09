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

namespace card_index_DAL.Repositories
{
    /// <summary>
    /// Repository to work with Genres data set
    /// </summary>
    public class GenreRepository : IGenreRepository
    {
        private readonly CardIndexDbContext _db;
        /// <summary>
        /// Repository constructor, takes database context as parameter
        /// </summary>
        /// <param name="context">DB context</param>
        public GenreRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        /// <summary>
        /// Gets all genre instances from DB
        /// </summary>
        /// <returns>Genres collection</returns>
        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _db.Genres.OrderBy(g => g.Id).ToListAsync();
        }
        /// <summary>
        /// Gets genre with given id from DB
        /// </summary>
        /// <param name="id">Genre identifier to search</param>
        /// <returns>Genre matching criteria</returns>
        public async Task<Genre> GetByIdAsync(int id)
        {
            return await _db.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }
        /// <summary>
        /// Adds genre to DB
        /// </summary>
        /// <param name="entity">New genre to add</param>
        /// <returns>Async operation</returns>
        public async Task AddAsync(Genre entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var alreadyExists = await _db.Genres.FirstOrDefaultAsync(g => g.Id == entity.Id);
            if (alreadyExists == null)
            {
                _db.Genres.Add(entity);
            }
            else
            {
                throw new EntityAlreadyExistsException($"Genre with id: {entity.Id} already exists");
            }
        }
        /// <summary>
        /// Deletes given genre from DB
        /// </summary>
        /// <param name="entity">Genre to delete</param>
        public void Delete(Genre entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var itemToDelete = _db.Genres.FirstOrDefault(g => g.Id == entity.Id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Genre with id: {entity.Id} not found in db");

            _db.Genres.Remove(itemToDelete);
        }
        /// <summary>
        /// Deletes genre with given id from DB
        /// </summary>
        /// <param name="id">Id of genre to delete</param>
        /// <returns>Async operation</returns>
        public async Task DeleteByIdAsync(int id)
        {
            var itemToDelete = await _db.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Genre with id: {id} not found in db)");

            _db.Genres.Remove(itemToDelete);
        }
        /// <summary>
        /// Updates given genre in DB
        /// </summary>
        /// <param name="entity">Genre to update</param>
        public void Update(Genre entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var existsInDb = _db.Genres.Any(g => g.Id == entity.Id);

            if (!existsInDb)
                throw new EntityNotFoundException($"Genre with id: {entity.Id} not found in db");

            _db.Genres.Update(entity);
        }
        /// <summary>
        /// Gets total number of genres,
        /// stored in database
        /// </summary>
        /// <returns>Number of genres</returns>
        public async Task<int> GetTotalNumberAsync()
        {
            return await _db.Genres.CountAsync();
        }
        /// <summary>
        /// Takes all genres, without connected instances,
        /// filtered by paging parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>Genres collection</returns>
        public async Task<IEnumerable<Genre>> GetAllAsync(PagingParameters parameters)
        {
            return await _db.Genres
                .OrderBy(g => g.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
        /// <summary>
        /// Gets genre with all connected entities matching given id
        /// </summary>
        /// <param name="id">Genre id to search</param>
        /// <returns>Genre object</returns>
        public async Task<Genre> GetByIdWithDetailsAsync(int id)
        {
            return await _db.Genres.Include(g => g.TextCards)
                .ThenInclude(tc => tc.Authors)
                .Include(g => g.TextCards)
                .ThenInclude(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .OrderBy(g => g.Id)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
        /// <summary>
        /// Gets genre with all connected entities matching given name
        /// </summary>
        /// <param name="name">Genre name to search</param>
        /// <returns>Genre object</returns>
        public async Task<Genre> GetByNameWithDetailsAsync(string name)
        {
            return await _db.Genres.Include(g => g.TextCards)
                .ThenInclude(tc => tc.Authors)
                .Include(g => g.TextCards)
                .ThenInclude(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .OrderBy(g => g.Id)
                .FirstOrDefaultAsync(g => g.Title == name);
        }
        /// <summary>
        /// Takes all genres, including other instances connected,
        /// filtered by paging parameters
        /// </summary>
        /// <param name="parameters">Paging filter parameters</param>
        /// <returns>Genres collection</returns>
        public async Task<IEnumerable<Genre>> GetAllWithDetailsAsync(PagingParameters parameters)
        {
            return await _db.Genres
                .OrderBy(g => g.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Include(g => g.TextCards)
                .ThenInclude(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .Include(tc => tc.TextCards)
                .ThenInclude(tc => tc.Authors)
                .ToListAsync();
        }
    }
}
