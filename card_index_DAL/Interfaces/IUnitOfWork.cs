using System.Threading.Tasks;

namespace card_index_DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }
        IGenreRepository GenreRepository { get; }
        IRateDetailRepository RateDetailRepository { get; }
        ITextCardRepository TextCardRepository { get; }
        //UserManager<User> UserManager { get; }
        //RoleManager<UserRole> RoleManager { get; }
        Task SaveChangesAsync();
    }
}
