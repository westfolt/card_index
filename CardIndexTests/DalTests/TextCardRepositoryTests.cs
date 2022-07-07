using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;
using card_index_DAL.Entities.DataShapingModels;
using card_index_DAL.Exceptions;
using card_index_DAL.Repositories;
using CardIndexTests.DalTests.Helpers;
using CardIndexTests.Helpers;
using NUnit.Framework;

namespace CardIndexTests.DalTests
{
    [TestFixture]
    public class TextCardRepositoryTests
    {
        private CardIndexDbContext _context;
        private TextCardRepository _cardRepository;
        private DalTestsData _data;
        private IEnumerable<TextCard> _expectedTextCards;
        private IEnumerable<TextCard> _expectedTextCardsWithDetails;

        [SetUp]
        public void Initialize()
        {
            _context = new CardIndexDbContext(DbTestHelper.GetTestDbOptions());
            _cardRepository = new TextCardRepository(_context);
            _data = new DalTestsData();
            _expectedTextCards = _data.ExpectedTextCards;
            _expectedTextCardsWithDetails = _data.ExpectedTextCardsWithDetails;
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task TextCardRepository_GetAllAsync_ReturnsAllTextCards()
        {
            var cards = await _cardRepository.GetAllAsync();

            Assert.That(cards, Is.EqualTo(_expectedTextCards).Using(new TextCardComparer()));
        }

        [Test]
        public async Task TextCardRepository_GetAllAsync_ReturnsAllTextCardsByPage()
        {
            var filter = new CardFilter { PageSize = 2, PageNumber = 2 };
            var expected = _expectedTextCards.Skip(2).Take(2);
            var actual = await _cardRepository.GetAllAsync(filter);

            Assert.That(expected, Is.EqualTo(actual).Using(new TextCardComparer()));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task TextCardRepository_GetByIdAsync_ReturnsTextCard(int id)
        {
            var card = await _cardRepository.GetByIdAsync(id);

            Assert.That(card, Is.EqualTo(_expectedTextCards.ToList()[id - 1]).Using(new TextCardComparer()));
        }

        [Test]
        public async Task TextCardRepository_AddAsync_AddsValueToDatabase()
        {
            var cardToAdd = new TextCard()
            {
                Title = "newTextCard",
                ReleaseDate = new DateTime(1970, 1, 1),
                CardRating = 5,
                GenreId = 2,
            };

            await _cardRepository.AddAsync(cardToAdd);
            await _context.SaveChangesAsync();

            Assert.That(_context.TextCards.Count(), Is.EqualTo(_expectedTextCards.Count() + 1));
        }

        [Test]
        public async Task TextCardRepository_AddAsync_ReturnsExceptionOnNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _cardRepository.AddAsync(null));
        }

        [Test]
        public async Task TextCardRepository_AddAsync_ReturnsExceptionOnDuplicate()
        {
            var cardToAdd = _expectedTextCards.Last();

            Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await _cardRepository.AddAsync(cardToAdd));
        }

        [Test]
        public async Task TextCardRepository_Delete_DeletesValueFromDatabase()
        {
            var cardToDelete = _expectedTextCards.Last();

            _cardRepository.Delete(cardToDelete);
            await _context.SaveChangesAsync();

            Assert.That(_context.TextCards.Count(), Is.EqualTo(_expectedTextCards.Count() - 1));
        }

        [Test]
        public async Task TextCardRepository_Delete_ReturnsExceptionOnNotNull()
        {
            Assert.Throws<ArgumentNullException>(() => _cardRepository.Delete(null));
        }

        [Test]
        public async Task TextCardRepository_Delete_ReturnsExceptionOnNotFound()
        {
            var genreToAdd = new TextCard()
            {
                Id = 999,
                Title = "newTextCard",
                ReleaseDate = new DateTime(1970, 1, 1),
                CardRating = 5,
                GenreId = 2,
            };

            Assert.Throws<EntityNotFoundException>(() => _cardRepository.Delete(genreToAdd));
        }

        [Test]
        public async Task TextCardRepository_DeleteByIdAsync_DeletesValueFromDb()
        {
            var idToDelete = _expectedTextCards.Last().Id;

            await _cardRepository.DeleteByIdAsync(idToDelete);
            await _context.SaveChangesAsync();

            Assert.That(_context.TextCards.Count(), Is.EqualTo(_expectedTextCards.Count() - 1));
        }

        [Test]
        public async Task TextCardRepository_DeleteByIdAsync_ReturnsExceptionOnNotFound()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _cardRepository.DeleteByIdAsync(999));
        }

        [Test]
        public async Task TextCardRepository_Update_UpdatesEntity()
        {
            var newTextCard = new TextCard
            {
                Id = 1,
                Title = "newTextCard",
                ReleaseDate = new DateTime(1970, 1, 1),
                CardRating = 5,
                GenreId = 2,
            };

            _cardRepository.Update(newTextCard);
            await _context.SaveChangesAsync();
            var actual = await _cardRepository.GetByIdAsync(newTextCard.Id);

            Assert.That(actual, Is.EqualTo(newTextCard));
        }

        [Test]
        public async Task TextCardRepository_Update_ReturnsExceptionOnNotFound()
        {
            var newTextCard = new TextCard
            {
                Id = 999,
                Title = "newTextCard",
                ReleaseDate = new DateTime(1970, 1, 1),
                CardRating = 5,
                GenreId = 2,
            };

            Assert.Throws<EntityNotFoundException>(() => _cardRepository.Update(newTextCard));
        }

        [Test]
        public async Task TextCardRepository_Update_ReturnsExceptionOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => _cardRepository.Update(null));
        }

        [Test]
        public async Task TextCardRepository_GetTotalNumberAsync_ReturnsTextCardsCount()
        {
            Assert.That(await _cardRepository.GetTotalNumberAsync(), Is.EqualTo(_expectedTextCards.Count()));
        }

        [Test]
        public async Task TextCardRepository_GetTotalNumberByFilterAsync_ReturnsTextCardsCount()
        {
            var filter = new CardFilter
                { AuthorId = 0, GenreId = 0, CardName = "", PageNumber = 1, PageSize = 30, Rating = 0 };

            Assert.That(await _cardRepository.GetTotalNumberByFilterAsync(filter), Is.EqualTo(_expectedTextCards.Count()));
        }

        [Test]
        public async Task TextCardRepository_GetAllWithDetailsAsync_ReturnsAllTextCardsWithDetails()
        {
            var expected = _expectedTextCardsWithDetails;

            var actual = await _cardRepository.GetAllWithDetailsAsync();
            Assert.That(actual, Is.EqualTo(expected).Using(new TextCardComparer()));
        }

        [Test]
        public async Task TextCardRepository_GetAllWithDetailsAsync_ReturnsAllTextCardsWithDetailsByPage()
        {
            var filter = new CardFilter { PageSize = 2, PageNumber = 2 };
            var expected = _expectedTextCardsWithDetails.Skip(2).Take(2);

            var actual = await _cardRepository.GetAllWithDetailsAsync(filter);
            Assert.That(actual, Is.EqualTo(expected).Using(new TextCardComparer()));
        }

        [Test]
        public async Task TextCardRepository_GetByIdWithDetailsAsync_ReturnsTextCardWithDetails()
        {
            var expected = _expectedTextCardsWithDetails.First();

            var actual = await _cardRepository.GetByIdWithDetailsAsync(1);
            Assert.That(actual, Is.EqualTo(expected).Using(new TextCardComparer()));
        }
    }
}
