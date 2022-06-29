using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using card_index_BLL.Exceptions;
using card_index_BLL.Services;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;
using CardIndexTests.BllTests.Helpers;
using CardIndexTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace CardIndexTests.BllTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private readonly DataForEntityTests _data = DataForEntityTests.GetTestData();

        [Test]
        public async Task UserService_GetAll_ReturnsAllUsers()
        {
            var expected = _data.AuthorDtos;
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null);
            var mockRoleManager =
                new Mock<RoleManager<UserRole>>(Mock.Of<IRoleStore<UserRole>>(), null, null, null, null);
            IQueryable<User> queryableUsers = _data.Users.AsQueryable();
            mockUserManager
                .Setup(x => x.Users)
                .Returns(queryableUsers);

            mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) => _data.Roles.Where(r => r.Id == u.Id).Select(r => r.Name).ToList());
            
            var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUserManager.Object,
                mockRoleManager.Object);

            var actual = await UserService.GetAllAsync();

            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.TextCardIds));
        }
        [Test]
        public async Task UserService_GetAll_ReturnsCardIndexException()
        {
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockRoleManager =
                new Mock<RoleManager<UserRole>>(Mock.Of<IRoleStore<UserRole>>(), null, null, null, null);

            mockUserManager
                .Setup(x => x.Users)
                .Throws(new Exception());

            var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUserManager.Object,
                mockRoleManager.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await UserService.GetAllAsync());
        }
        //[TestCase(1)]
        //[TestCase(2)]
        //public async Task UserService_GetById_ReturnsAuthor(int id)
        //{
        //    var expected = _data.AuthorDtos.ToList()[id];
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    mockUnitOfWork
        //        .Setup(x => x.UserRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
        //        .ReturnsAsync(_data.UserInfoModels.ToList()[id]);

        //    var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

        //    var actual = await UserService.GetByIdAsync(id);

        //    actual.Should().BeEquivalentTo(expected, options =>
        //        options.Excluding(x => x.TextCardIds));
        //}
        //[TestCase(1)]
        //[TestCase(2)]
        //public async Task UserService_GetById_ReturnsCardIndexException(int id)
        //{
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    mockUnitOfWork
        //        .Setup(x => x.UserRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
        //        .Throws(new Exception());

        //    var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

        //    Assert.ThrowsAsync<CardIndexException>(async () => await UserService.GetByIdAsync(id));
        //}
        //[Test]
        //public async Task UserService_AddAsync_AddsAuthor()
        //{
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(x => x.UserRepository.AddAsync(It.IsAny<Author>()));

        //    var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);
        //    var author = _data.AuthorDtos.First();

        //    var resultId = await UserService.AddAsync(author);

        //    mockUnitOfWork.Verify(x => x.UserRepository.AddAsync(It.Is<Author>(
        //        c => c.Id == 0 &&
        //             c.FirstName == author.FirstName &&
        //             c.LastName == author.LastName &&
        //             c.YearOfBirth == author.YearOfBirth)), Times.Once);

        //    mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        //}
        //[Test]
        //public async Task UserService_AddAsync_ReturnsCardIndexException()
        //{
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();
        //    var author = _data.AuthorDtos.First();

        //    mockUnitOfWork
        //        .Setup(x => x.UserRepository.AddAsync(It.IsAny<Author>()))
        //        .Throws(new Exception());

        //    var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

        //    Assert.ThrowsAsync<CardIndexException>(async () => await UserService.AddAsync(author));
        //}
        //[Test]
        //public async Task UserService_UpdateAsync_UpdatesAuthor()
        //{
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(m => m.UserRepository.Update(It.IsAny<Author>()));

        //    var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);
        //    var author = _data.AuthorDtos.First();

        //    await UserService.UpdateAsync(author);

        //    mockUnitOfWork.Verify(x => x.UserRepository.Update(It.Is<Author>(
        //        c => c.Id == author.Id &&
        //             c.FirstName == author.FirstName &&
        //             c.LastName == author.LastName &&
        //             c.YearOfBirth == author.YearOfBirth)), Times.Once);
        //    mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        //}
        //[Test]
        //public async Task UserService_UpdateAsync_ReturnsCardIndexException()
        //{
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();
        //    var author = _data.AuthorDtos.First();

        //    mockUnitOfWork
        //        .Setup(x => x.UserRepository.Update(It.IsAny<Author>()))
        //        .Throws(new Exception());

        //    var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

        //    Assert.ThrowsAsync<CardIndexException>(async () => await UserService.UpdateAsync(author));
        //}
        //[TestCase(1)]
        //[TestCase(2)]
        //public async Task UserService_DeleteAsync_DeletesProduct(int id)
        //{
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(m => m.UserRepository.DeleteByIdAsync(It.IsAny<int>()));
        //    var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

        //    await UserService.DeleteAsync(id);

        //    mockUnitOfWork.Verify(x => x.UserRepository.DeleteByIdAsync(id), Times.Once);
        //    mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        //}
        //[TestCase(1)]
        //[TestCase(2)]
        //public async Task UserService_DeleteAsync_ReturnsCardIndexException(int id)
        //{
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    mockUnitOfWork
        //        .Setup(x => x.UserRepository.DeleteByIdAsync(It.IsAny<int>()))
        //        .Throws(new Exception());

        //    var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

        //    Assert.ThrowsAsync<CardIndexException>(async () => await UserService.DeleteAsync(id));
        //}
        //[TestCase(1981, 1983, new[] { 1, 2, 3 })]
        //[TestCase(1983, 1985, new[] { 3, 4, 5 })]
        //[TestCase(1982, 1983, new[] { 2, 3 })]
        //public async Task UserService_GetUsersForPeriodAsync_ReturnsCardIndexException(int startYear, int endYear, int[] expectedAuthorIds)
        //{
        //    //arrange
        //    var expected = _data.AuthorDtos.Where(x => expectedAuthorIds.Contains(x.Id));

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    mockUnitOfWork
        //        .Setup(x => x.UserRepository.GetAllWithDetailsAsync())
        //        .ReturnsAsync(_data.UserInfoModels.AsEnumerable());

        //    var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

        //    //act
        //    var actual = await UserService.GetUsersForPeriodAsync(startYear, endYear);

        //    //assert
        //    actual.Should().BeEquivalentTo(expected, options =>
        //        options.Excluding(x => x.TextCardIds)
        //    );
        //}
        //[TestCase(1981, 1983)]
        //[TestCase(1983, 1985)]
        //[TestCase(1982, 1983)]
        //public async Task UserService_GetUsersForPeriodAsync_ReturnsUsersByCategory(int startYear, int endYear)
        //{
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    mockUnitOfWork
        //        .Setup(x => x.UserRepository.GetAllWithDetailsAsync())
        //        .Throws(new Exception());

        //    var UserService = new UserService(DbTestHelper.CreateMapperProfile(), mockUnitOfWork.Object);

        //    Assert.ThrowsAsync<CardIndexException>(async () => await UserService.GetUsersForPeriodAsync(startYear, endYear));
        //}
    }
}
