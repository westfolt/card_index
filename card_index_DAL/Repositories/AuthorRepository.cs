using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Exceptions;
using card_index_DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace card_index_DAL.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly CardIndexDbContext _db;

        public AuthorRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _db.Authors.ToListAsync();
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            return await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }

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

        public void Delete(Author entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var itemToDelete = _db.Authors.FirstOrDefault(a => a.Id == entity.Id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Author with id: {entity.Id} not found in db");

            _db.Authors.Remove(itemToDelete);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var itemToDelete = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (itemToDelete == null)
                throw new EntityNotFoundException($"Author with id: {id} not found in db)");

            _db.Authors.Remove(itemToDelete);
        }

        public void Update(Author entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Given entity is null");

            var existsInDb = _db.Authors.Any(a => a.Id == entity.Id);

            if (!existsInDb)
                throw new EntityNotFoundException($"Author with id: {entity.Id} not found in db");

            _db.Authors.Update(entity);
        }

        public async Task<IEnumerable<Author>> GetAllWithDetailsAsync()
        {
            return await _db.Authors.Include(a => a.TextCards)
                .ThenInclude(tc => tc.Genre)
                .Include(a => a.TextCards)
                .ThenInclude(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .ToListAsync();
        }

        public async Task<Author> GetByIdWithDetailsAsync(int id)
        {
            return await _db.Authors.Include(a => a.TextCards)
                .ThenInclude(tc => tc.Genre)
                .Include(a => a.TextCards)
                .ThenInclude(tc => tc.RateDetails)
                .ThenInclude(rd => rd.User)
                .FirstOrDefaultAsync(a=>a.Id == id);
        }
    }
}
