using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;

namespace card_index_DAL.Repositories
{
    public class AccountRepository:IAccountRepository
    {
        private readonly CardIndexDbContext _db;

        public AccountRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        public Task<IEnumerable<AppUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(AppUser entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(AppUser entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(AppUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AppUser>> GetAllWithDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> GetByIdWithDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
