using card_index_BLL.Exceptions;
using card_index_BLL.Models;
using card_index_BLL.Models.Dto;
using card_index_BLL.Services;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;
using CardIndexTests.BllTests.Helpers;
using CardIndexTests.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardIndexTests.BllTests
{
    [TestFixture]
    public class CardServiceTests
    {
        private readonly DataForEntityTests _data = DataForEntityTests.GetTestData();

        [Test]
        public async Task CardService_GetAll_ReturnsAllTextCards()
        {
            var expected = _data.TextCardDtos;
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.TextCardRepository.GetAllWithDetailsAsync())
                .ReturnsAsync(_data.TextCards.AsEnumerable());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            var actual = await cardService.GetAllAsync();

            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.RateDetailsIds)
                    .Excluding(x => x.AuthorIds)
                    .Excluding(x => x.GenreName));
        }
        [Test]
        public async Task CardService_GetAll_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.TextCardRepository.GetAllWithDetailsAsync())
                .Throws(new Exception());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await cardService.GetAllAsync());
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task CardService_GetById_ReturnsTextCard(int id)
        {
            var expected = _data.TextCardDtos.ToList()[id];
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.TextCardRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(_data.TextCards.ToList()[id]);

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            var actual = await cardService.GetByIdAsync(id);

            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.RateDetailsIds)
                    .Excluding(x => x.AuthorIds)
                    .Excluding(x => x.GenreName));
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task CardService_GetById_ReturnsCardIndexException(int id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.TextCardRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .Throws(new Exception());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await cardService.GetByIdAsync(id));
        }
        [Test]
        public async Task CardService_AddAsync_AddsTextCard()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.TextCardRepository.AddAsync(It.IsAny<TextCard>()));
            mockUnitOfWork.Setup(x => x.GenreRepository.GetAllAsync())
                .ReturnsAsync(new List<Genre>());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);
            var cardDto = _data.TextCardDtos.First();

            await cardService.AddAsync(cardDto);

            mockUnitOfWork.Verify(x => x.TextCardRepository.AddAsync(It.Is<TextCard>(
                c => c.Id == 0 &&
                     c.Title == cardDto.Title &&
                     c.ReleaseDate == cardDto.ReleaseDate &&
                     Math.Abs(c.CardRating - cardDto.CardRating) < 10E-6)), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task CardService_AddAsync_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var author = _data.TextCardDtos.First();

            mockUnitOfWork
                .Setup(x => x.TextCardRepository.AddAsync(It.IsAny<TextCard>()))
                .Throws(new Exception());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await cardService.AddAsync(author));
        }
        [Test]
        public async Task CardService_UpdateAsync_UpdatesTextCard()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.TextCardRepository.Update(It.IsAny<TextCard>()));
            mockUnitOfWork.Setup(x => x.GenreRepository.GetAllAsync())
                .ReturnsAsync(new List<Genre>());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);
            var cardDto = _data.TextCardDtos.First();

            await cardService.UpdateAsync(cardDto);

            mockUnitOfWork.Verify(x => x.TextCardRepository.Update(It.Is<TextCard>(
                c => c.Id == cardDto.Id &&
                     c.Title == cardDto.Title &&
                     c.ReleaseDate == cardDto.ReleaseDate &&
                     Math.Abs(c.CardRating - cardDto.CardRating) < 10E-6)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task CardService_UpdateAsync_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var author = _data.TextCardDtos.First();

            mockUnitOfWork
                .Setup(x => x.TextCardRepository.Update(It.IsAny<TextCard>()))
                .Throws(new Exception());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await cardService.UpdateAsync(author));
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task CardService_DeleteAsync_DeletesProduct(int id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.TextCardRepository.DeleteByIdAsync(It.IsAny<int>()));
            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            await cardService.DeleteAsync(id);

            mockUnitOfWork.Verify(x => x.TextCardRepository.DeleteByIdAsync(id), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task CardService_DeleteAsync_ReturnsCardIndexException(int id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.TextCardRepository.DeleteByIdAsync(It.IsAny<int>()))
                .Throws(new Exception());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await cardService.DeleteAsync(id));
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task CardService_CalculateCardRatingAsync_ReturnsCardRating(int cardIndex)
        {
            var expected = _data.TextCardDtos.ToList()[cardIndex - 1].CardRating;

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.RateDetailRepository.GetAllAsync())
                .ReturnsAsync(_data.RateDetails.AsEnumerable());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            var actual = await cardService.CalculateCardRatingAsync(cardIndex);

            Assert.That(actual, Is.EqualTo(expected));
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task CardService_CalculateCardRatingAsync_ReturnsCardIndexException(int cardIndex)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.RateDetailRepository.GetAllAsync())
                .Throws(new Exception());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await cardService.CalculateCardRatingAsync(cardIndex));
        }
        [TestCase(1990, 1991, new[] { 1, 2, 3, 4 })]
        public async Task CardService_GetCardsForPeriodAsync_ReturnsCards(int startYear, int endYear, int[] expectedCardIds)
        {
            var expected = _data.TextCardDtos.Where(x => expectedCardIds.Contains(x.Id));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.TextCardRepository.GetAllWithDetailsAsync())
                .ReturnsAsync(_data.TextCards.AsEnumerable());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            var actual =
                await cardService.GetCardsForPeriodAsync(new DateTime(startYear, 1, 1), new DateTime(endYear, 12, 31));

            actual.Should().BeEquivalentTo(expected, options =>
                    options.Excluding(x => x.RateDetailsIds)
                        .Excluding(x => x.AuthorIds)
                        .Excluding(x => x.GenreName));
        }
        [Test]
        public async Task CardService_GetCardsForPeriodAsync_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.TextCardRepository.GetAllWithDetailsAsync())
                .Throws(new Exception());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await cardService.GetCardsForPeriodAsync(new DateTime(1999, 9, 9), new DateTime(2000, 1, 1)));
        }
        [TestCase(1, 1, 4.0, new[] { 2 })]
        public async Task CardService_GetCardsByFilter_ReturnsCards(int authorId, int genreId, double rating, int[] expectedCardIds)
        {
            var expected = _data.TextCardDtos.Where(x => expectedCardIds.Contains(x.Id));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.TextCardRepository.GetAllWithDetailsAsync())
                .ReturnsAsync(_data.TextCards.AsEnumerable());

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            var actual =
                await cardService.GetCardsByFilterAsync(new FilterModel
                { AuthorId = null, GenreId = genreId, Rating = rating });

            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.RateDetailsIds)
                    .Excluding(x => x.AuthorIds)
                    .Excluding(x => x.GenreName));
        }
        [Test]
        public async Task CardService_GetCardsByFilter_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await cardService.GetCardsByFilterAsync(null));
        }
        [Test]
        public async Task CardService_AddRatingToCard_AddsRating()
        {
            var rateDetailToAdd = new RateDetailDto
            {
                UserId = 3,
                TextCardId = 1
            };
            var cardToModify = new TextCard
            {
                CardRating = 0
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.RateDetailRepository.AddAsync(It.IsAny<RateDetail>()));
            mockUnitOfWork
                .Setup(m => m.RateDetailRepository.GetAllAsync())
                .ReturnsAsync(_data.RateDetails);
            mockUnitOfWork
                .Setup(m => m.TextCardRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(cardToModify);

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            await cardService.AddRatingToCard(rateDetailToAdd);

            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));
            Assert.That(cardToModify.CardRating, Is.Not.EqualTo(0));
        }
        [Test]
        public async Task CardService_AddRatingToCard_ReturnsCardIndexException()
        {
            var modelWithError = _data.RateDetailDtos.First();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.RateDetailRepository.GetAllAsync())
                .ReturnsAsync(_data.RateDetails.AsEnumerable);


            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await cardService.AddRatingToCard(modelWithError));
        }
        [Test()]
        public async Task CardService_DeleteRatingFromCard_DeletesRating()
        {
            var rateDetailToAdd = new RateDetailDto
            {
                UserId = 1,
                TextCardId = 1
            };
            var cardToModify = new TextCard
            {
                CardRating = 0
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.RateDetailRepository.AddAsync(It.IsAny<RateDetail>()));
            mockUnitOfWork
                .Setup(m => m.RateDetailRepository.GetAllAsync())
                .ReturnsAsync(_data.RateDetails);
            mockUnitOfWork
                .Setup(m => m.TextCardRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(cardToModify);

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            await cardService.DeleteRatingFromCard(1, 1);

            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));
            Assert.That(cardToModify.CardRating, Is.Not.EqualTo(0));
        }
        [Test]
        public async Task CardService_DeleteRatingFromCard_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.RateDetailRepository.GetAllAsync())
                .ReturnsAsync(_data.RateDetails.AsEnumerable);

            var cardService = new CardService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await cardService.DeleteRatingFromCard(1, 1));
        }
    }
}
