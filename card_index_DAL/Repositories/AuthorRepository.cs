using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;

namespace card_index_DAL.Repositories
{
    public class AuthorRepository:IAuthorRepository
    {
        private readonly CardIndexDbContext _db;

        public AuthorRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        public Task<IEnumerable<Author>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(Author entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Author entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Author entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Author>> GetAllWithDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetByIdWithDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
