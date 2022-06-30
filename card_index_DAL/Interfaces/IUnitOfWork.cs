using System.Threading.Tasks;

namespace card_index_DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }
        IGenreRepository GenreRepository { get; }
        IRateDetailRepository RateDetailRepository { get; }
        ITextCardRepository TextCardRepository { get; }
        Task SaveChangesAsync();
    }
}
