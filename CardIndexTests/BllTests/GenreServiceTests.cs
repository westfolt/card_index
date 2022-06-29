using System;
using System.Linq;
using System.Threading.Tasks;
using card_index_BLL.Exceptions;
using card_index_BLL.Services;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;
using CardIndexTests.BllTests.Helpers;
using CardIndexTests.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CardIndexTests.BllTests
{
    [TestFixture]
    public class genreServiceTests
    {
        private readonly DataForEntityTests _data = DataForEntityTests.GetTestData();

        [Test]
        public async Task GenreService_GetAll_ReturnsAllGenres()
        {
            var expected = _data.GenreDtos;
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.GenreRepository.GetAllAsync())
                .ReturnsAsync(_data.Genres.AsEnumerable());

            var genreService = new GenreService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            var actual = await genreService.GetAllAsync();

            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.TextCardIds));
        }
        [Test]
        public async Task GenreService_GetAll_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.GenreRepository.GetAllAsync())
                .Throws(new Exception());

            var genreService = new GenreService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await genreService.GetAllAsync());
        }
        [TestCase("GenreOne",1)]
        [TestCase("GenreTwo", 2)]
        public async Task GenreService_GetByName_ReturnsGenre(string name, int id)
        {
            var expected = _data.GenreDtos.ToList()[id-1];
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.GenreRepository.GetAllAsync())
                .ReturnsAsync(_data.Genres.AsEnumerable());

            var genreService = new GenreService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            var actual = await genreService.GetByNameAsync(name);

            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.TextCardIds));
        }
        [TestCase("GenreOne", 1)]
        [TestCase("GenreTwo", 2)]
        public async Task GenreService_GetByName_ReturnsCardIndexException(string name, int id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.GenreRepository.GetAllAsync())
                .Throws(new Exception());

            var genreService = new GenreService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await genreService.GetByNameAsync(name));
        }
        [Test]
        public async Task GenreService_AddAsync_AddsGenre()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.GenreRepository.AddAsync(It.IsAny<Genre>()));

            var genreService = new GenreService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);
            var genre = _data.GenreDtos.First();

            var resultId = await genreService.AddAsync(genre);

            mockUnitOfWork.Verify(x => x.GenreRepository.AddAsync(It.Is<Genre>(
                c => c.Id == 0 &&
                     c.Title == genre.Title)), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task GenreService_AddAsync_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var Genre = _data.GenreDtos.First();

            mockUnitOfWork
                .Setup(x => x.GenreRepository.AddAsync(It.IsAny<Genre>()))
                .Throws(new Exception());

            var genreService = new GenreService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await genreService.AddAsync(Genre));
        }
        [Test]
        public async Task GenreService_UpdateAsync_UpdatesGenre()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.GenreRepository.Update(It.IsAny<Genre>()));

            var genreService = new GenreService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);
            var genre = _data.GenreDtos.First();

            await genreService.UpdateAsync(genre);

            mockUnitOfWork.Verify(x => x.GenreRepository.Update(It.Is<Genre>(
                c => c.Id == genre.Id)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task GenreService_UpdateAsync_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var genre = _data.GenreDtos.First();

            mockUnitOfWork
                .Setup(x => x.GenreRepository.Update(It.IsAny<Genre>()))
                .Throws(new Exception());

            var genreService = new GenreService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await genreService.UpdateAsync(genre));
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task GenreService_DeleteAsync_DeletesProduct(int id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.GenreRepository.DeleteByIdAsync(It.IsAny<int>()));
            var genreService = new GenreService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            await genreService.DeleteAsync(id);

            mockUnitOfWork.Verify(x => x.GenreRepository.DeleteByIdAsync(id), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task GenreService_DeleteAsync_ReturnsCardIndexException(int id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.GenreRepository.DeleteByIdAsync(It.IsAny<int>()))
                .Throws(new Exception());

            var genreService = new GenreService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await genreService.DeleteAsync(id));
        }
    }
}
