using card_index_DAL.Data;
using card_index_DAL.Interfaces;
using card_index_DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace card_index_DAL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CardIndexDbContext _dbContext;
        private IAuthorRepository _authorRepository;
        private IGenreRepository _genreRepository;
        private IRateDetailRepository _rateDetailRepository;
        private ITextCardRepository _textCardRepository;
        private bool disposed = false;

        public UnitOfWork(CardIndexDbContext context)
        {
            _dbContext = context;
        }

        public IAuthorRepository AuthorRepository
        {
            get { return _authorRepository ??= new AuthorRepository(_dbContext); }
        }

        public IGenreRepository GenreRepository
        {
            get { return _genreRepository ??= new GenreRepository(_dbContext); }
        }

        public IRateDetailRepository RateDetailRepository
        {
            get { return _rateDetailRepository ??= new RateDetailRepository(_dbContext); }
        }

        public ITextCardRepository TextCardRepository
        {
            get { return _textCardRepository ??= new TextCardRepository(_dbContext); }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
