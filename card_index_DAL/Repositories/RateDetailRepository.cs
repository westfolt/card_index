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
    public class RateDetailRepository : IRateDetailRepository
    {
        private readonly CardIndexDbContext _db;

        public RateDetailRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<RateDetail>> GetAllAsync()
        {
            return await _db.RateDetails.ToListAsync();
        }

        public async Task<RateDetail> GetByIdAsync(int id)
        {
            return await _db.RateDetails.FirstOrDefaultAsync(rd => rd.Id == id);
        }

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

        public void Delete(RateDetail entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var itemToDelete = _db.RateDetails.FirstOrDefault(rd => rd.Id == entity.Id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Rate detail with id: {entity.Id} not found in db");

            _db.RateDetails.Remove(itemToDelete);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var itemToDelete = await _db.RateDetails.FirstOrDefaultAsync(rd => rd.Id == id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Rate detail with id: {id} not found in db)");

            _db.RateDetails.Remove(itemToDelete);
        }

        public void Update(RateDetail entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var existsInDb = _db.RateDetails.Any(rd => rd.Id == entity.Id);

            if (!existsInDb)
                throw new EntityNotFoundException($"Rate detail with id: {entity.Id} not found in db");

            _db.RateDetails.Update(entity);
        }

        public async Task<int> GetTotalNumberAsync()
        {
            return await _db.RateDetails.CountAsync();
        }

        public async Task<IEnumerable<RateDetail>> GetAllWithDetailsAsync()
        {
            return await _db.RateDetails.Include(rd => rd.User)
                .Include(rd => rd.TextCard)
                .ThenInclude(tc => tc.Authors)
                .Include(rd => rd.TextCard)
                .ThenInclude(tc => tc.Genre)
                .ToListAsync();
        }
    }
}
