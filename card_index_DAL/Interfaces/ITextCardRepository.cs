using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Entities;

namespace card_index_DAL.Interfaces
{
    public interface ITextCardRepository:IRepository<TextCard>
    {
        Task<IEnumerable<TextCard>> GetAllWithDetailsAsync();
        Task<TextCard> GetByIdWithDetailsAsync(int id);
    }
}
