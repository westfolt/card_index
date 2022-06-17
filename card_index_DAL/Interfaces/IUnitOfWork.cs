using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace card_index_DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }
        IGenreRepository GenreRepository { get; }
        IRateDetailRepository RateDetailRepository { get; }
        ITextCardRepository TextCardRepository { get; }
        UserManager<User> UserManager { get; }
        RoleManager<UserRole> RoleManager { get; }
        Task SaveChangesAsync();
    }
}
