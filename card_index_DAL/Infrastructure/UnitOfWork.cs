using card_index_DAL.Data;
using card_index_DAL.Interfaces;
using card_index_DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace card_index_DAL.Infrastructure
{
    /// <summary>
    /// represents unit of work with database for BLL,
    /// takes all repositories instances and has method to perform
    /// DB changes save, implements IDisposable to dispose DB context
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CardIndexDbContext _dbContext;
        private IAuthorRepository _authorRepository;
        private IGenreRepository _genreRepository;
        private IRateDetailRepository _rateDetailRepository;
        private ITextCardRepository _textCardRepository;
        private bool disposed = false;
        /// <summary>
        /// Constructor, takes db context in
        /// </summary>
        /// <param name="context">DB context</param>
        public UnitOfWork(CardIndexDbContext context)
        {
            _dbContext = context;
        }
        /// <summary>
        /// Repository to work with Authors data set
        /// </summary>
        public IAuthorRepository AuthorRepository
        {
            get { return _authorRepository ??= new AuthorRepository(_dbContext); }
        }
        /// <summary>
        /// Repository to work with Genres data set
        /// </summary>
        public IGenreRepository GenreRepository
        {
            get { return _genreRepository ??= new GenreRepository(_dbContext); }
        }
        /// <summary>
        /// Repository to work with Rate details data set
        /// </summary>
        public IRateDetailRepository RateDetailRepository
        {
            get { return _rateDetailRepository ??= new RateDetailRepository(_dbContext); }
        }
        /// <summary>
        /// Repository to work with Text cards data set
        /// </summary>
        public ITextCardRepository TextCardRepository
        {
            get { return _textCardRepository ??= new TextCardRepository(_dbContext); }
        }
        /// <summary>
        /// Saves all changes to database
        /// </summary>
        /// <returns>Async operation</returns>
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Disposes db context
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Disposes db context
        /// </summary>
        /// <param name="disposing">indicates disposing</param>
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
