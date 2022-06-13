using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;

namespace card_index_DAL.Repositories
{
    public class UserRoleRepository:IUserRoleRepository
    {
        private readonly CardIndexDbContext _db;

        public UserRoleRepository(CardIndexDbContext context)
        {
            _db = context;
        }
        public Task<IEnumerable<AppUserRole>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AppUserRole> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(AppUserRole entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(AppUserRole entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(AppUserRole entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AppUserRole>> GetAllWithDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AppUserRole> GetByIdWithDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
