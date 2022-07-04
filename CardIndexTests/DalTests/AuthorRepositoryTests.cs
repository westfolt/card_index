using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;
using card_index_DAL.Exceptions;
using card_index_DAL.Repositories;
using CardIndexTests.Helpers;
using NUnit.Framework;

namespace CardIndexTests.DalTests
{
    [TestFixture]
    public class AuthorRepositoryTests
    {
        private CardIndexDbContext _context;
        private AuthorRepository _authorRepository;

        [SetUp]
        public void Initialize()
        {
            _context = new CardIndexDbContext(DbTestHelper.GetTestDbOptions());
            _authorRepository = new AuthorRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task AuthorRepository_GetAllAsync_ReturnsAllAuthors()
        {
            var authors = await _authorRepository.GetAllAsync();

            Assert.That(authors, Is.EqualTo(ExpectedAuthors).Using(new AuthorComparer()));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task AuthorRepository_GetByIdAsync_ReturnsAuthor(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            Assert.That(author, Is.EqualTo(ExpectedAuthors.ToList()[id - 1]).Using(new AuthorComparer()));
        }

        [Test]
        public async Task AuthorRepository_AddAsync_AddsValueToDatabase()
        {
            var authorToAdd = new Author() { FirstName = "newAuthor", LastName = "newAuthor", YearOfBirth = 2000 };

            await _authorRepository.AddAsync(authorToAdd);
            await _context.SaveChangesAsync();

            Assert.That(_context.Authors.Count(), Is.EqualTo(ExpectedAuthors.Count() + 1));
        }

        [Test]
        public async Task AuthorRepository_AddAsync_ReturnsExceptionOnNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _authorRepository.AddAsync(null));
        }

        [Test]
        public async Task AuthorRepository_AddAsync_ReturnsExceptionOnDuplicate()
        {
            var authorToAdd = ExpectedAuthors.Last();

            Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await _authorRepository.AddAsync(authorToAdd));
        }

        [Test]
        public async Task AuthorRepository_Delete_DeletesValueFromDatabase()
        {
            var authorToDelete = ExpectedAuthors.Last();

            _authorRepository.Delete(authorToDelete);
            await _context.SaveChangesAsync();

            Assert.That(_context.Authors.Count(), Is.EqualTo(ExpectedAuthors.Count() - 1));
        }

        [Test]
        public async Task AuthorRepository_Delete_ReturnsExceptionOnNotNull()
        {
            Assert.Throws<ArgumentNullException>(() => _authorRepository.Delete(null));
        }

        [Test]
        public async Task AuthorRepository_Delete_ReturnsExceptionOnNotFound()
        {
            var authorToDelete = new Author() { FirstName = "newAuthor", LastName = "newAuthor", YearOfBirth = 2000 };

            Assert.Throws<EntityNotFoundException>(() => _authorRepository.Delete(authorToDelete));
        }

        [Test]
        public async Task AuthorRepository_DeleteByIdAsync_DeletesValueFromDb()
        {
            var idToDelete = ExpectedAuthors.Last().Id;

            await _authorRepository.DeleteByIdAsync(idToDelete);
            await _context.SaveChangesAsync();

            Assert.That(_context.Authors.Count(), Is.EqualTo(ExpectedAuthors.Count() - 1));
        }

        [Test]
        public async Task AuthorRepository_DeleteByIdAsync_ReturnsExceptionOnNotFound()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _authorRepository.DeleteByIdAsync(999));
        }

        [Test]
        public async Task AuthorRepository_Update_UpdatesEntity()
        {
            var newAuthor = new Author { Id = 1, FirstName = "newAuthor", LastName = "newAuthor", YearOfBirth = 2000 };

            _authorRepository.Update(newAuthor);
            await _context.SaveChangesAsync();
            var actual = await _authorRepository.GetByIdAsync(newAuthor.Id);

            Assert.That(actual, Is.EqualTo(newAuthor));
        }

        [Test]
        public async Task AuthorRepository_Update_ReturnsExceptionOnNotFound()
        {
            var newAuthor = new Author { Id = 999, FirstName = "newAuthor", LastName = "newAuthor", YearOfBirth = 2000 };

            Assert.Throws<EntityNotFoundException>(() => _authorRepository.Update(newAuthor));
        }

        [Test]
        public async Task AuthorRepository_Update_ReturnsExceptionOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => _authorRepository.Update(null));
        }

        [Test]
        public async Task AuthorRepository_GetTotalNumberAsync_ReturnsAuthorsCount()
        {
            Assert.That(await _authorRepository.GetTotalNumberAsync(), Is.EqualTo(ExpectedAuthors.Count()));
        }

        [Test]
        public async Task AuthorRepository_GetAllAsync_ReturnsAuthorsByPage()
        {
            var pageParameters = new PagingParameters { PageSize = 2, PageNumber = 2 };
            var expected = ExpectedAuthors.Skip(2).Take(2);

            var actual = await _authorRepository.GetAllAsync(pageParameters);

            Assert.That(actual, Is.EqualTo(expected).Using(new AuthorComparer()));
        }

        [Test]
        public async Task AuthorRepository_GetAllWithDetailsAsync_ReturnsAllAuthorsWithDetails()
        {
            var expected = ExpectedAuthorsWithDetails;

            var actual = await _authorRepository.GetAllWithDetailsAsync();
            Assert.That(actual, Is.EqualTo(expected).Using(new AuthorComparer()));
        }

        [Test]
        public async Task AuthorRepository_GetByIdWithDetailsAsync_ReturnsAuthorWithDetails()
        {
            var expected = ExpectedAuthorsWithDetails.First();

            var actual = await _authorRepository.GetByIdWithDetailsAsync(1);
            Assert.That(actual, Is.EqualTo(expected).Using(new AuthorComparer()));
        }

        [Test]
        public async Task AuthorRepository_GetAllWithDetailsAsync_ReturnsAllAuthorsWithDetailsByPage()
        {
            var pageParameters = new PagingParameters { PageSize = 2, PageNumber = 2 };
            var expected = ExpectedAuthorsWithDetails.Skip(2).Take(2);

            var actual = await _authorRepository.GetAllWithDetailsAsync(pageParameters);
            Assert.That(actual, Is.EqualTo(expected).Using(new AuthorComparer()));
        }


        private static IEnumerable<Author> ExpectedAuthors =>
            new[]
            {
                new Author { Id = 1, FirstName = "James", LastName = "Benton", YearOfBirth = 1956 },
                new Author { Id = 2, FirstName = "Donette", LastName = "Foller", YearOfBirth = 1989 },
                new Author { Id = 3, FirstName = "Veronika", LastName = "Donald", YearOfBirth = 1990 },
                new Author { Id = 4, FirstName = "Jack", LastName = "Wieser", YearOfBirth = 2000 },
                new Author { Id = 5, FirstName = "Arnold", LastName = "Clark", YearOfBirth = 2001 }
            };
        private static IEnumerable<Author> ExpectedAuthorsWithDetails =>
            new[]
            {
                new Author { Id = 1, FirstName = "James", LastName = "Benton", YearOfBirth = 1956, TextCards = new List<TextCard>{new TextCard()}},
                new Author { Id = 2, FirstName = "Donette", LastName = "Foller", YearOfBirth = 1989, TextCards = new List<TextCard>{new TextCard()}},
                new Author { Id = 3, FirstName = "Veronika", LastName = "Donald", YearOfBirth = 1990, TextCards = new List<TextCard>{new TextCard()}},
                new Author { Id = 4, FirstName = "Jack", LastName = "Wieser", YearOfBirth = 2000, TextCards = new List<TextCard>{new TextCard()}},
                new Author { Id = 5, FirstName = "Arnold", LastName = "Clark", YearOfBirth = 2001, TextCards = new List<TextCard>{new TextCard()}}
            };

    }
}
