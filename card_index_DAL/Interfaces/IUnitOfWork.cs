using System.Threading.Tasks;

namespace card_index_DAL.Interfaces
{
    /// <summary>
    /// represents unit of work with database for BLL,
    /// takes all repositories instances and has method to perform
    /// DB changes save
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Repository to work with Authors data set
        /// </summary>
        IAuthorRepository AuthorRepository { get; }
        /// <summary>
        /// Repository to work with Genres data set
        /// </summary>
        IGenreRepository GenreRepository { get; }
        /// <summary>
        /// Repository to work with Rate details data set
        /// </summary>
        IRateDetailRepository RateDetailRepository { get; }
        /// <summary>
        /// Repository to work with Text cards data set
        /// </summary>
        ITextCardRepository TextCardRepository { get; }
        /// <summary>
        /// Saves all changes to database
        /// </summary>
        /// <returns>Async operation</returns>
        Task SaveChangesAsync();
    }
}
