using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;
using card_index_DAL.Exceptions;
using card_index_DAL.Repositories;
using CardIndexTests.DalTests.Helpers;
using CardIndexTests.Helpers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using NUnit.Framework;

namespace CardIndexTests.DalTests
{
    [TestFixture]
    public class GenreRepositoryTests
    {
        private CardIndexDbContext _context;
        private GenreRepository _genreRepository;
        private DalTestsData _data;
        private IEnumerable<Genre> _expectedGenres;
        private IEnumerable<Genre> _expectedGenresWithDetails;

        [SetUp]
        public void Initialize()
        {
            _context = new CardIndexDbContext(DbTestHelper.GetTestDbOptions());
            _genreRepository = new GenreRepository(_context);
            _data = new DalTestsData();
            _expectedGenres = _data.ExpectedGenres;
            _expectedGenresWithDetails = _data.ExpectedGenresWithDetails;
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GenreRepository_GetAllAsync_ReturnsAllGenres()
        {
            var genres = await _genreRepository.GetAllAsync();

            Assert.That(genres, Is.EqualTo(_expectedGenres).Using(new GenreComparer()));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GenreRepository_GetByIdAsync_ReturnsGenre(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);

            Assert.That(genre, Is.EqualTo(_expectedGenres.ToList()[id-1]).Using(new GenreComparer()));
        }

        [Test]
        public async Task GenreRepository_AddAsync_AddsValueToDatabase()
        {
            var genreToAdd = new Genre() { Title = "newGenre" };

            await _genreRepository.AddAsync(genreToAdd);
            await _context.SaveChangesAsync();

            Assert.That(_context.Genres.Count(), Is.EqualTo(_expectedGenres.Count() + 1));
        }

        [Test]
        public async Task GenreRepository_AddAsync_ReturnsExceptionOnNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _genreRepository.AddAsync(null));
        }

        [Test]
        public async Task GenreRepository_AddAsync_ReturnsExceptionOnDuplicate()
        {
            var genreToAdd = _expectedGenres.Last();

            Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await _genreRepository.AddAsync(genreToAdd));
        }

        [Test]
        public async Task GenreRepository_Delete_DeletesValueFromDatabase()
        {
            var genreToDelete = _expectedGenres.Last();

            _genreRepository.Delete(genreToDelete);
            await _context.SaveChangesAsync();

            Assert.That(_context.Genres.Count(), Is.EqualTo(_expectedGenres.Count() - 1));
        }

        [Test]
        public async Task GenreRepository_Delete_ReturnsExceptionOnNotNull()
        {
            Assert.Throws<ArgumentNullException>(() => _genreRepository.Delete(null));
        }

        [Test]
        public async Task GenreRepository_Delete_ReturnsExceptionOnNotFound()
        {
            var genreToAdd = new Genre() { Id = 999, Title = "newGenre" };

            Assert.Throws<EntityNotFoundException>(() => _genreRepository.Delete(genreToAdd));
        }

        [Test]
        public async Task GenreRepository_DeleteByIdAsync_DeletesValueFromDb()
        {
            var idToDelete = _expectedGenres.Last().Id;

            await _genreRepository.DeleteByIdAsync(idToDelete);
            await _context.SaveChangesAsync();

            Assert.That(_context.Genres.Count(), Is.EqualTo(_expectedGenres.Count() - 1));
        }

        [Test]
        public async Task GenreRepository_DeleteByIdAsync_ReturnsExceptionOnNotFound()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _genreRepository.DeleteByIdAsync(999));
        }

        [Test]
        public async Task GenreRepository_Update_UpdatesEntity()
        {
            var newGenre = new Genre { Id = 1, Title = "newGenre" };

            _genreRepository.Update(newGenre);
            await _context.SaveChangesAsync();
            var actual = await _genreRepository.GetByIdAsync(newGenre.Id);

            Assert.That(actual, Is.EqualTo(newGenre));
        }

        [Test]
        public async Task GenreRepository_Update_ReturnsExceptionOnNotFound()
        {
            var newGenre = new Genre { Id = 999, Title = "newGenre" };

            Assert.Throws<EntityNotFoundException>(() => _genreRepository.Update(newGenre));
        }

        [Test]
        public async Task GenreRepository_Update_ReturnsExceptionOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => _genreRepository.Update(null));
        }

        [Test]
        public async Task GenreRepository_GetTotalNumberAsync_ReturnsGenresCount()
        {
            Assert.That(await _genreRepository.GetTotalNumberAsync(), Is.EqualTo(_expectedGenres.Count()));
        }

        [Test]
        public async Task GenreRepository_GetAllAsync_ReturnsGenresByPage()
        {
            var pageParameters = new PagingParameters { PageSize = 2, PageNumber = 2 };
            var expected = _expectedGenres.Skip(2).Take(2);

            var actual = await _genreRepository.GetAllAsync(pageParameters);

            Assert.That(actual.OrderBy(g=>g.Id), Is.EqualTo(expected).Using(new GenreComparer()));
        }

        [Test]
        public async Task GenreRepository_GetByIdWithDetailsAsync_ReturnsGenreWithDetails()
        {
            var expected = _expectedGenres.First();
            expected.TextCards = new List<TextCard>
            {
                new TextCard 
                { 
                    Title = "Card1",
                    ReleaseDate = new DateTime(1980, 3, 3),
                    CardRating = 0,
                    GenreId = 1
                }
            };

            var actual = await _genreRepository.GetByIdWithDetailsAsync(1);
            Assert.That(actual, Is.EqualTo(expected).Using(new GenreComparer()));
        }

        [Test]
        public async Task GenreRepository_GetByNameWithDetailsAsync_ReturnsGenreWithDetails()
        {
            var expected = _expectedGenresWithDetails.First();

            var actual = await _genreRepository.GetByNameWithDetailsAsync("Genre1");
            Assert.That(actual, Is.EqualTo(expected).Using(new GenreComparer()));
        }

        [Test]
        public async Task GenreRepository_GetAllWithDetailsAsync_ReturnsAllGenresWithDetailsByPage()
        {
            var pageParameters = new PagingParameters { PageSize = 2, PageNumber = 2 };
            var expected = _expectedGenresWithDetails.Skip(2).Take(2);

            var actual = await _genreRepository.GetAllWithDetailsAsync(pageParameters);
            Assert.That(actual, Is.EqualTo(expected).Using(new GenreComparer()));
        }
    }
}
