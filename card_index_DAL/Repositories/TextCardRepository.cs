using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;

namespace card_index_DAL.Repositories
{
    public class TextCardRepository:ITextCardRepository
    {
        private readonly CardIndexDbContext _db;

        public TextCardRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        public Task<IEnumerable<TextCard>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TextCard> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(TextCard entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TextCard entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(TextCard entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TextCard>> GetAllWithDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TextCard> GetByIdWithDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
