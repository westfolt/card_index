using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;

namespace card_index_DAL.Repositories
{
    public class GenreRepository:IGenreRepository
    {
        private readonly CardIndexDbContext _db;

        public GenreRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        public Task<IEnumerable<Genre>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Genre> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(Genre entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Genre entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Genre entity)
        {
            throw new NotImplementedException();
        }
    }
}
