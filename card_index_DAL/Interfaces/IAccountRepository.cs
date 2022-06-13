using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Entities;

namespace card_index_DAL.Interfaces
{
    public interface IAccountRepository:IRepository<AppUser>
    {
        Task<IEnumerable<AppUser>> GetAllWithDetailsAsync();
        Task<AppUser> GetByIdWithDetailsAsync(int id);
    }
}
