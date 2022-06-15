using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;
using card_index_DAL.Repositories;
using Microsoft.AspNetCore.Identity;

namespace card_index_DAL.Infrastructure
{
    public class UnitOfWork:IUnitOfWork, IDisposable
    {
        private readonly CardIndexDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private IAuthorRepository _authorRepository;
        private IGenreRepository _genreRepository;
        private IRateDetailRepository _rateDetailRepository;
        private ITextCardRepository _textCardRepository;
        private bool disposed = false;

        public UnitOfWork(CardIndexDbContext context, UserManager<User> userManager, RoleManager<UserRole> roleManager)
        {
            _dbContext = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

        public UserManager<User> UserManager => _userManager;
        public RoleManager<UserRole> RoleManager => _roleManager;

        public async Task SaveAsync()
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
                    _userManager.Dispose();
                    _roleManager.Dispose();
                    _dbContext.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
