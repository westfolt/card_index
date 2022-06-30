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
    public class TextCardRepository : ITextCardRepository
    {
        private readonly CardIndexDbContext _db;

        public TextCardRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<TextCard>> GetAllAsync()
        {
            return await _db.TextCards.ToListAsync();
        }
        public async Task<IEnumerable<TextCard>> GetAllAsync(PagingParameters parameters)
        {
            return await _db.TextCards
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        public async Task<TextCard> GetByIdAsync(int id)
        {
            return await _db.TextCards.FirstOrDefaultAsync(tc => tc.Id == id);
        }

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

        public void Delete(TextCard entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var itemToDelete = _db.TextCards.FirstOrDefault(tc => tc.Id == entity.Id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"TextCard with id: {entity.Id} not found in db");

            _db.TextCards.Remove(itemToDelete);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var itemToDelete = await _db.TextCards.FirstOrDefaultAsync(tc => tc.Id == id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"TextCard with id: {id} not found in db)");

            _db.TextCards.Remove(itemToDelete);
        }

        public void Update(TextCard entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var existsInDb = _db.TextCards.Any(tc => tc.Id == entity.Id);

            if (!existsInDb)
                throw new EntityNotFoundException($"TextCard with id: {entity.Id} not found in db");

            _db.TextCards.Update(entity);
        }

        public async Task<int> GetTotalNumberAsync()
        {
            return await _db.TextCards.CountAsync();
        }

        public async Task<IEnumerable<TextCard>> GetAllWithDetailsAsync()
        {
            return await _db.TextCards.Include(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .Include(tc => tc.Genre)
                .Include(tc => tc.Authors)
                .ToListAsync();
        }

        public async Task<IEnumerable<TextCard>> GetAllWithDetailsAsync(PagingParameters parameters)
        {
            return await _db.TextCards
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Include(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .Include(tc => tc.Genre)
                .Include(tc => tc.Authors)
                .ToListAsync();
        }

        public async Task<TextCard> GetByIdWithDetailsAsync(int id)
        {
            return await _db.TextCards.Include(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .Include(tc => tc.Genre)
                .Include(tc => tc.Authors)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }
    }
}
