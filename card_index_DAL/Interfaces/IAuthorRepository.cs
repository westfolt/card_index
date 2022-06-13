using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Entities;

namespace card_index_DAL.Interfaces
{
    public interface IAuthorRepository:IRepository<Author>
    {
        Task<IEnumerable<Author>> GetAllWithDetailsAsync();
        Task<Author> GetByIdWithDetailsAsync(int id);
    }
}
