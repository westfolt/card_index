using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Exceptions;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Models;
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
    public class AuthorServiceTests
    {
        private readonly DataForEntityTests _data = DataForEntityTests.GetTestData();
        
        [Test]
        public async Task AuthorService_GetAll_ReturnsAllAuthors()
        {
            var expected = _data.AuthorDtos;
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.AuthorRepository.GetAllWithDetailsAsync())
                .ReturnsAsync(_data.Authors.AsEnumerable());

            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);
            
            var actual = await authorService.GetAllAsync();
            
            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.TextCardIds));
        }
        [Test]
        public async Task AuthorService_GetAll_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.AuthorRepository.GetAllWithDetailsAsync())
                .Throws(new Exception());

            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);
            
            Assert.ThrowsAsync<CardIndexException>(async()=> await authorService.GetAllAsync());
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task AuthorService_GetById_ReturnsAuthor(int id)
        {
            var expected = _data.AuthorDtos.ToList()[id];
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.AuthorRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(_data.Authors.ToList()[id]);

            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            var actual = await authorService.GetByIdAsync(id);

            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.TextCardIds));
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task AuthorService_GetById_ReturnsCardIndexException(int id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.AuthorRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .Throws(new Exception());

            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await authorService.GetByIdAsync(id));
        }
        [Test]
        public async Task AuthorService_AddAsync_AddsAuthor()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.AuthorRepository.AddAsync(It.IsAny<Author>()));

            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);
            var author = _data.AuthorDtos.First();
            
            var resultId = await authorService.AddAsync(author);
            
            mockUnitOfWork.Verify(x => x.AuthorRepository.AddAsync(It.Is<Author>(
                c => c.Id == 0 &&
                     c.FirstName == author.FirstName &&
                     c.LastName == author.LastName &&
                     c.YearOfBirth == author.YearOfBirth)), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task AuthorService_AddAsync_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var author = _data.AuthorDtos.First();

            mockUnitOfWork
                .Setup(x => x.AuthorRepository.AddAsync(It.IsAny<Author>()))
                .Throws(new Exception());

            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await authorService.AddAsync(author));
        }
        [Test]
        public async Task AuthorService_UpdateAsync_UpdatesAuthor()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.AuthorRepository.Update(It.IsAny<Author>()));

            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);
            var author = _data.AuthorDtos.First();
            
            await authorService.UpdateAsync(author);
            
            mockUnitOfWork.Verify(x => x.AuthorRepository.Update(It.Is<Author>(
                c => c.Id == author.Id &&
                     c.FirstName == author.FirstName &&
                     c.LastName == author.LastName &&
                     c.YearOfBirth == author.YearOfBirth)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task AuthorService_UpdateAsync_ReturnsCardIndexException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var author = _data.AuthorDtos.First();

            mockUnitOfWork
                .Setup(x => x.AuthorRepository.Update(It.IsAny<Author>()))
                .Throws(new Exception());

            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await authorService.UpdateAsync(author));
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task AuthorService_DeleteAsync_DeletesProduct(int id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.AuthorRepository.DeleteByIdAsync(It.IsAny<int>()));
            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);
            
            await authorService.DeleteAsync(id);
            
            mockUnitOfWork.Verify(x => x.AuthorRepository.DeleteByIdAsync(id), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task AuthorService_DeleteAsync_ReturnsCardIndexException(int id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.AuthorRepository.DeleteByIdAsync(It.IsAny<int>()))
                .Throws(new Exception());

            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await authorService.DeleteAsync(id));
        }
        [TestCase(1981, 1983, new[] { 1, 2, 3 })]
        [TestCase(1983, 1985, new[] { 3, 4, 5 })]
        [TestCase(1982, 1983, new[] { 2, 3 })]
        public async Task AuthorService_GetAuthorsForPeriodAsync_ReturnsAuthors(int startYear, int endYear, int[] expectedAuthorIds)
        {
            //arrange
            var expected = _data.AuthorDtos.Where(x => expectedAuthorIds.Contains(x.Id));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.AuthorRepository.GetAllWithDetailsAsync())
                .ReturnsAsync(_data.Authors.AsEnumerable());

            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            //act
            var actual = await authorService.GetAuthorsForPeriodAsync(startYear,endYear);

            //assert
            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.TextCardIds)
            );
        }
        [TestCase(1981, 1983)]
        [TestCase(1983, 1985)]
        [TestCase(1982, 1983)]
        public async Task AuthorService_GetAuthorsForPeriodAsync_ReturnsCardIndexException(int startYear, int endYear)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            
            mockUnitOfWork
                .Setup(x => x.AuthorRepository.GetAllWithDetailsAsync())
                .Throws(new Exception());

            var authorService = new AuthorService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await authorService.GetAuthorsForPeriodAsync(startYear, endYear));
        }
    }
}
