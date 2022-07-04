using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Exceptions;
using card_index_DAL.Repositories;
using CardIndexTests.DalTests.Helpers;
using CardIndexTests.Helpers;
using NUnit.Framework;

namespace CardIndexTests.DalTests
{
    [TestFixture]
    public class RateDetailRepositoryTests
    {
        private CardIndexDbContext _context;
        private RateDetailRepository _rateDetailRepository;
        private DalTestsData _data;
        private IEnumerable<RateDetail> _expectedRateDetails;
        private IEnumerable<RateDetail> _expectedRateDetailsWithDetails;

        [SetUp]
        public async Task Initialize()
        {
            //RateDetail rate1 = new RateDetail
            //{
            //    UserId = 1,
            //    TextCardId = 1,
            //    RateValue = 3
            //};
            //RateDetail rate2 = new RateDetail
            //{
            //    UserId = 2,
            //    TextCardId = 1,
            //    RateValue = 5
            //};
            //RateDetail rate3 = new RateDetail
            //{
            //    UserId = 1,
            //    TextCardId = 2,
            //    RateValue = 3
            //};
            //var user1 = new User()
            //{
            //    FirstName = "Oleksandr",
            //    LastName = "Shyman",
            //    Email = "mymail@gmail.com",
            //    City = "Rivne",
            //    UserName = "mymail@gmail.com",
            //    DateOfBirth = new DateTime(1988, 12, 12),
            //    PhoneNumber = "+38(012)3456789"
            //};
            //var user2 = new User()
            //{
            //    FirstName = "Aleksey",
            //    LastName = "Grishkov",
            //    Email = "newmail@gmail.com",
            //    City = "Rivne",
            //    UserName = "newmail@gmail.com",
            //    DateOfBirth = new DateTime(2001, 01, 12),
            //    PhoneNumber = "+38(012)3456789"
            //};
            //var user3 = new User()
            //{
            //    FirstName = "Taras",
            //    LastName = "Bobkin",
            //    Email = "taras@gmail.com",
            //    City = "Rivne",
            //    UserName = "taras@gmail.com",
            //    DateOfBirth = new DateTime(1976, 01, 12),
            //    PhoneNumber = "+38(012)3456789"
            //};
            _context = new CardIndexDbContext(DbTestHelper.GetTestDbOptions());
            //await _context.RateDetails.AddRangeAsync(rate1, rate2, rate3);
            //await _context.SaveChangesAsync();
            //await _context.Users.AddRangeAsync(user1, user2, user3);
            //await _context.SaveChangesAsync();
            _rateDetailRepository = new RateDetailRepository(_context);
            _data = new DalTestsData();
            _expectedRateDetails = _data.ExpectedRateDetails;
            _expectedRateDetailsWithDetails = _data.ExpectedRateDetailsWithDetails;
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task RateDetailRepository_GetAllAsync_ReturnsAllRateDetails()
        {
            var rateDetails = await _rateDetailRepository.GetAllAsync();

            Assert.That(rateDetails, Is.EqualTo(_expectedRateDetails).Using(new RateDetailComparer()));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task RateDetailRepository_GetByIdAsync_ReturnsRateDetail(int id)
        {
            var rateDetails = await _rateDetailRepository.GetByIdAsync(id);

            Assert.That(rateDetails, Is.EqualTo(_expectedRateDetails.ToList()[id - 1]).Using(new RateDetailComparer()));
        }

        [Test]
        public async Task RateDetailRepository_AddAsync_AddsValueToDatabase()
        {
            var rateToAdd = new RateDetail() { RateValue = 3, TextCardId = 1, UserId = 1 };

            await _rateDetailRepository.AddAsync(rateToAdd);
            await _context.SaveChangesAsync();

            Assert.That(_context.RateDetails.Count(), Is.EqualTo(_expectedRateDetails.Count() + 1));
        }

        [Test]
        public async Task RateDetailRepository_AddAsync_ReturnsExceptionOnNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _rateDetailRepository.AddAsync(null));
        }

        [Test]
        public async Task RateDetailRepository_AddAsync_ReturnsExceptionOnDuplicate()
        {
            var authorToAdd = _expectedRateDetails.Last();

            Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await _rateDetailRepository.AddAsync(authorToAdd));
        }

        [Test]
        public async Task RateDetailRepository_Delete_DeletesValueFromDatabase()
        {
            var rateToDelete = _expectedRateDetails.Last();

            _rateDetailRepository.Delete(rateToDelete);
            await _context.SaveChangesAsync();

            Assert.That(_context.RateDetails.Count(), Is.EqualTo(_expectedRateDetails.Count() - 1));
        }

        [Test]
        public async Task RateDetailRepository_Delete_ReturnsExceptionOnNotNull()
        {
            Assert.Throws<ArgumentNullException>(() => _rateDetailRepository.Delete(null));
        }

        [Test]
        public async Task RateDetailRepository_Delete_ReturnsExceptionOnNotFound()
        {
            var rateToDelete = new RateDetail() { Id = 999, RateValue = 3, TextCardId = 5, UserId = 5 };

            Assert.Throws<EntityNotFoundException>(() => _rateDetailRepository.Delete(rateToDelete));
        }

        [Test]
        public async Task RateDetailRepository_Update_UpdatesEntity()
        {
            var newRateDetail = new RateDetail { Id = 1, UserId = 2, TextCardId = 2, RateValue = 4 };

            _rateDetailRepository.Update(newRateDetail);
            await _context.SaveChangesAsync();
            var actual = await _rateDetailRepository.GetByIdAsync(newRateDetail.Id);

            Assert.That(actual, Is.EqualTo(newRateDetail));
        }

        [Test]
        public async Task RateDetailRepository_Update_ReturnsExceptionOnNotFound()
        {
            var newRateDetail = new RateDetail() { Id = 999, RateValue = 3, TextCardId = 5, UserId = 5 };

            Assert.Throws<EntityNotFoundException>(() => _rateDetailRepository.Update(newRateDetail));
        }

        [Test]
        public async Task RateDetailRepository_Update_ReturnsExceptionOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => _rateDetailRepository.Update(null));
        }

        [Test]
        public async Task RateDetailRepository_GetTotalNumberAsync_ReturnsRateDetailsCount()
        {
            Assert.That(await _rateDetailRepository.GetTotalNumberAsync(), Is.EqualTo(_expectedRateDetails.Count()));
        }

        [Test]
        public async Task RateDetailRepository_GetAllWithDetailsAsync_ReturnsAllRateDetailsWithDetails()
        {
            var expected = _expectedRateDetailsWithDetails;

            var actual = await _rateDetailRepository.GetAllWithDetailsAsync();
            Assert.That(actual, Is.EqualTo(expected).Using(new RateDetailComparer()));
        }

        [Test]
        public async Task RateDetailRepository_GetByIdWithDetailsAsync_ReturnsRateDetailWithDetails()
        {
            var expected = _expectedRateDetailsWithDetails.First();

            var actual = await _rateDetailRepository.GetByIdWithDetailsAsync(1);
            Assert.That(actual, Is.EqualTo(expected).Using(new RateDetailComparer()));
        }
    }
}
