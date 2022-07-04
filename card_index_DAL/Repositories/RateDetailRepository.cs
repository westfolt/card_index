using card_index_DAL.Data;
using card_index_DAL.Entities;
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
    /// Repository to work with Rate details data set
    /// </summary>
    public class RateDetailRepository : IRateDetailRepository
    {
        private readonly CardIndexDbContext _db;
        /// <summary>
        /// Repository constructor, takes database context as parameter
        /// </summary>
        /// <param name="context">DB context</param>
        public RateDetailRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        /// <summary>
        /// Gets all instances of rate details from DB
        /// </summary>
        /// <returns>Rate details collection</returns>
        public async Task<IEnumerable<RateDetail>> GetAllAsync()
        {
            return await _db.RateDetails.OrderBy(rd=>rd.Id).ToListAsync();
        }
        /// <summary>
        /// Gets rate detail with given id from DB
        /// </summary>
        /// <param name="id">Rate details identifier to search</param>
        /// <returns>Rate detail matching criteria</returns>
        public async Task<RateDetail> GetByIdAsync(int id)
        {
            return await _db.RateDetails.FirstOrDefaultAsync(rd => rd.Id == id);
        }
        /// <summary>
        /// Adds rate detail to DB
        /// </summary>
        /// <param name="entity">New rate detail to add</param>
        /// <returns>Async operation</returns>
        public async Task AddAsync(RateDetail entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var alreadyExists = await _db.RateDetails.FirstOrDefaultAsync(rd => rd.Id == entity.Id);
            if (alreadyExists == null)
            {
                _db.RateDetails.Add(entity);
            }
            else
            {
                throw new EntityAlreadyExistsException($"Rate detail with id: {entity.Id} already exists");
            }
        }
        /// <summary>
        /// Deletes given rate detail from DB
        /// </summary>
        /// <param name="entity">Rate detail to delete</param>
        public void Delete(RateDetail entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var itemToDelete = _db.RateDetails.FirstOrDefault(rd => rd.Id == entity.Id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Rate detail with id: {entity.Id} not found in db");

            _db.RateDetails.Remove(itemToDelete);
        }
        /// <summary>
        /// Deletes rate detail with given id and type from DB
        /// </summary>
        /// <param name="id">Id of rate detail to delete</param>
        /// <returns>Async operation</returns>
        public async Task DeleteByIdAsync(int id)
        {
            var itemToDelete = await _db.RateDetails.FirstOrDefaultAsync(rd => rd.Id == id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Rate detail with id: {id} not found in db)");

            _db.RateDetails.Remove(itemToDelete);
        }
        /// <summary>
        /// Updates given rate detail in DB
        /// </summary>
        /// <param name="entity">Rate detail to update</param>
        public void Update(RateDetail entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var existsInDb = _db.RateDetails.Any(rd => rd.Id == entity.Id);

            if (!existsInDb)
                throw new EntityNotFoundException($"Rate detail with id: {entity.Id} not found in db");

            _db.RateDetails.Update(entity);
        }
        /// <summary>
        /// Gets total number of rate details,
        /// stored in database
        /// </summary>
        /// <returns>Number of rate details</returns>
        public async Task<int> GetTotalNumberAsync()
        {
            return await _db.RateDetails.CountAsync();
        }
        /// <summary>
        /// Gets all rate details with connected instances included
        /// </summary>
        /// <returns>Rate details list</returns>
        public async Task<IEnumerable<RateDetail>> GetAllWithDetailsAsync()
        {
            return await _db.RateDetails
                .Include(rd => rd.User)
                .Include(rd => rd.TextCard)
                .ThenInclude(tc => tc.Authors)
                .Include(rd => rd.TextCard)
                .ThenInclude(tc => tc.Genre)
                .OrderBy(rd => rd.Id)
                .ToListAsync();
        }
        /// <summary>
        /// Gets rate detail with connected instances included,
        /// matching given id
        /// </summary>
        /// <param name="id">Id of rate detail to search</param>
        /// <returns>Rate detail object</returns>
        public async Task<RateDetail> GetByIdWithDetailsAsync(int id)
        {
            return await _db.RateDetails.Include(rd => rd.User)
                .Include(rd => rd.TextCard)
                .ThenInclude(tc => tc.Authors)
                .Include(rd => rd.TextCard)
                .ThenInclude(tc => tc.Genre)
                .FirstOrDefaultAsync(rd => rd.Id == id);
        }
    }
}
