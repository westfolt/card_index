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
using CardIndexTests.DalTests.Helpers;
using CardIndexTests.Helpers;
using NUnit.Framework;

namespace CardIndexTests.DalTests
{
    [TestFixture]
    public class AuthorRepositoryTests
    {
        private CardIndexDbContext _context;
        private AuthorRepository _authorRepository;
        private DalTestsData _data;
        private IEnumerable<Author> _expectedAuthors;
        private IEnumerable<Author> _expectedAuthorsWithDetails;

        [SetUp]
        public void Initialize()
        {
            _context = new CardIndexDbContext(DbTestHelper.GetTestDbOptions());
            _authorRepository = new AuthorRepository(_context);
            _data = new DalTestsData();
            _expectedAuthors = _data.ExpectedAuthors;
            _expectedAuthorsWithDetails = _data.ExpectedAuthorsWithDetails;
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

            Assert.That(authors, Is.EqualTo(_expectedAuthors).Using(new AuthorComparer()));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task AuthorRepository_GetByIdAsync_ReturnsAuthor(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            Assert.That(author, Is.EqualTo(_expectedAuthors.ToList()[id - 1]).Using(new AuthorComparer()));
        }

        [Test]
        public async Task AuthorRepository_AddAsync_AddsValueToDatabase()
        {
            var authorToAdd = new Author() { FirstName = "newAuthor", LastName = "newAuthor", YearOfBirth = 2000 };

            await _authorRepository.AddAsync(authorToAdd);
            await _context.SaveChangesAsync();

            Assert.That(_context.Authors.Count(), Is.EqualTo(_expectedAuthors.Count() + 1));
        }

        [Test]
        public async Task AuthorRepository_AddAsync_ReturnsExceptionOnNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _authorRepository.AddAsync(null));
        }

        [Test]
        public async Task AuthorRepository_AddAsync_ReturnsExceptionOnDuplicate()
        {
            var authorToAdd = _expectedAuthors.Last();

            Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await _authorRepository.AddAsync(authorToAdd));
        }

        [Test]
        public async Task AuthorRepository_Delete_DeletesValueFromDatabase()
        {
            var authorToDelete = _expectedAuthors.Last();

            _authorRepository.Delete(authorToDelete);
            await _context.SaveChangesAsync();

            Assert.That(_context.Authors.Count(), Is.EqualTo(_expectedAuthors.Count() - 1));
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
            var idToDelete = _expectedAuthors.Last().Id;

            await _authorRepository.DeleteByIdAsync(idToDelete);
            await _context.SaveChangesAsync();

            Assert.That(_context.Authors.Count(), Is.EqualTo(_expectedAuthors.Count() - 1));
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
            Assert.That(await _authorRepository.GetTotalNumberAsync(), Is.EqualTo(_expectedAuthors.Count()));
        }

        [Test]
        public async Task AuthorRepository_GetAllAsync_ReturnsAuthorsByPage()
        {
            var pageParameters = new PagingParameters { PageSize = 2, PageNumber = 2 };
            var expected = _expectedAuthors.Skip(2).Take(2);

            var actual = await _authorRepository.GetAllAsync(pageParameters);

            Assert.That(actual, Is.EqualTo(expected).Using(new AuthorComparer()));
        }

        [Test]
        public async Task AuthorRepository_GetAllWithDetailsAsync_ReturnsAllAuthorsWithDetails()
        {
            var expected = _expectedAuthorsWithDetails;

            var actual = await _authorRepository.GetAllWithDetailsAsync();
            Assert.That(actual, Is.EqualTo(expected).Using(new AuthorComparer()));
        }

        [Test]
        public async Task AuthorRepository_GetByIdWithDetailsAsync_ReturnsAuthorWithDetails()
        {
            var expected = _expectedAuthorsWithDetails.First();

            var actual = await _authorRepository.GetByIdWithDetailsAsync(1);
            Assert.That(actual, Is.EqualTo(expected).Using(new AuthorComparer()));
        }

        [Test]
        public async Task AuthorRepository_GetAllWithDetailsAsync_ReturnsAllAuthorsWithDetailsByPage()
        {
            var pageParameters = new PagingParameters { PageSize = 2, PageNumber = 2 };
            var expected = _expectedAuthorsWithDetails.Skip(2).Take(2);

            var actual = await _authorRepository.GetAllWithDetailsAsync(pageParameters);
            Assert.That(actual, Is.EqualTo(expected).Using(new AuthorComparer()));
        }
    }
}
