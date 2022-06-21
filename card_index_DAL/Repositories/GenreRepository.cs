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
    public class GenreRepository : IGenreRepository
    {
        private readonly CardIndexDbContext _db;

        public GenreRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _db.Genres.ToListAsync();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            return await _db.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }

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

        public void Delete(Genre entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var itemToDelete = _db.Genres.FirstOrDefault(g => g.Id == entity.Id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Genre with id: {entity.Id} not found in db");

            _db.Genres.Remove(itemToDelete);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var itemToDelete = await _db.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Genre with id: {id} not found in db)");

            _db.Genres.Remove(itemToDelete);
        }

        public void Update(Genre entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var existsInDb = _db.Genres.Any(g => g.Id == entity.Id);

            if (!existsInDb)
                throw new EntityNotFoundException($"Genre with id: {entity.Id} not found in db");

            _db.Genres.Update(entity);
        }
    }
}
