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
    /// Repository to work with Authors data set
    /// </summary>
    public class AuthorRepository : IAuthorRepository
    {
        private readonly CardIndexDbContext _db;
        /// <summary>
        /// Repository constructor, takes database context as parameter
        /// </summary>
        /// <param name="context">DB context</param>
        public AuthorRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        /// <summary>
        /// Gets all author of given type from DB
        /// </summary>
        /// <returns>Authors collection of given type</returns>
        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _db.Authors.OrderBy(a => a.Id).ToListAsync();
        }
        /// <summary>
        /// Gets author with given id from DB
        /// </summary>
        /// <param name="id">Author identifier to search</param>
        /// <returns>Author matching criteria</returns>
        public async Task<Author> GetByIdAsync(int id)
        {
            return await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }
        /// <summary>
        /// Adds author to DB
        /// </summary>
        /// <param name="entity">New author to add</param>
        /// <returns>Async operation</returns>
        public async Task AddAsync(Author entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var alreadyExists = await _db.Authors.FirstOrDefaultAsync(a => a.Id == entity.Id);
            if (alreadyExists == null)
            {
                _db.Authors.Add(entity);
            }
            else
            {
                throw new EntityAlreadyExistsException($"Author with id: {entity.Id} already exists");
            }
        }
        /// <summary>
        /// Deletes given author from DB
        /// </summary>
        /// <param name="entity">Author to delete</param>
        public void Delete(Author entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var itemToDelete = _db.Authors.FirstOrDefault(a => a.Id == entity.Id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Author with id: {entity.Id} not found in db");

            _db.Authors.Remove(itemToDelete);
        }
        /// <summary>
        /// Deletes author with given id from DB
        /// </summary>
        /// <param name="id">Id of author to delete</param>
        /// <returns>Async operation</returns>
        public async Task DeleteByIdAsync(int id)
        {
            var itemToDelete = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Author with id: {id} not found in db)");

            _db.Authors.Remove(itemToDelete);
        }
        /// <summary>
        /// Updates given author in DB
        /// </summary>
        /// <param name="entity">Author to update</param>
        public void Update(Author entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var existsInDb = _db.Authors.Any(a => a.Id == entity.Id);

            if (!existsInDb)
                throw new EntityNotFoundException($"Author with id: {entity.Id} not found in db");

            _db.Authors.Update(entity);
        }
        /// <summary>
        /// Gets total number of authors,
        /// stored in database
        /// </summary>
        /// <returns>Number of authors</returns>
        public async Task<int> GetTotalNumberAsync()
        {
            return await _db.Authors.CountAsync();
        }
        /// <summary>
        /// Gets all author entities without connected objects
        /// included from DB
        /// </summary>
        /// <returns>Author entities collection</returns>
        public async Task<IEnumerable<Author>> GetAllAsync(PagingParameters parameters)
        {
            return await _db.Authors
                .OrderBy(a => a.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
        /// <summary>
        /// Gets all author entities with connected objects
        /// included from DB
        /// </summary>
        /// <returns>Author entities collection</returns>
        public async Task<IEnumerable<Author>> GetAllWithDetailsAsync()
        {
            return await _db.Authors.Include(a => a.TextCards)
                .ThenInclude(tc => tc.Genre)
                .Include(a => a.TextCards)
                .ThenInclude(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .OrderBy(a => a.Id)
                .ToListAsync();
        }
        /// <summary>
        /// Gets author with given id from DB,
        /// includes all connected entities details
        /// </summary>
        /// <param name="id">Author id to search</param>
        /// <returns>Author entity, matching criteria</returns>
        public async Task<Author> GetByIdWithDetailsAsync(int id)
        {
            return await _db.Authors.Include(a => a.TextCards)
                .ThenInclude(tc => tc.Genre)
                .Include(a => a.TextCards)
                .ThenInclude(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        /// <summary>
        /// Gets all author entities with connected objects
        /// included from DB, matching given paging filter
        /// </summary>
        /// <returns>Author entities collection</returns>
        public async Task<IEnumerable<Author>> GetAllWithDetailsAsync(PagingParameters parameters)
        {
            return await _db.Authors
                .OrderBy(a => a.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Include(a => a.TextCards)
                .ThenInclude(tc => tc.Genre)
                .Include(a => a.TextCards)
                .ThenInclude(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .ToListAsync();
        }
    }
}
