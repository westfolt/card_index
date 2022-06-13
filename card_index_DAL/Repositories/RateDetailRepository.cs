using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;

namespace card_index_DAL.Repositories
{
    public class RateDetailRepository:IRateDetailRepository
    {
        private readonly CardIndexDbContext _db;

        public RateDetailRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        public Task<IEnumerable<RateDetail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RateDetail> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(RateDetail entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(RateDetail entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(RateDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}
