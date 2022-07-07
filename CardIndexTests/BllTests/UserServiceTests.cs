using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Services;
using card_index_DAL.Entities;
using CardIndexTests.BllTests.Helpers;
using CardIndexTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using card_index_BLL.Models.Identity.Models;

namespace CardIndexTests.BllTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private readonly DataForEntityTests _data = DataForEntityTests.GetTestData();

        [Test]
        public async Task UserService_GetAll_ReturnsAllUsers()
        {
            var expected = _data.UserInfoModels;
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetUsersUMAsync())
                .ReturnsAsync(_data.Users.ToList());
            mockUsersRolesManager
                .Setup(x => x.GetRolesFromUserManagerAsync(It.IsAny<User>()))
                .ReturnsAsync(_data.RoleInfoModels.Select(r => r.RoleName).ToList());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            var actual = await userService.GetAllAsync();

            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.UserRoles));
        }

        [Test]
        public async Task UserService_GetAll_ReturnsCardIndexException()
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetUsersUMAsync())
                .Throws(new Exception());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await userService.GetAllAsync());
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserService_GetById_ReturnsUser(int id)
        {
            var expected = _data.UserInfoModels.ToList()[id - 1];
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetUsersUMAsync())
                .ReturnsAsync(_data.Users.ToList());
            mockUsersRolesManager
                .Setup(x => x.GetRolesFromUserManagerAsync(It.IsAny<User>()))
                .ReturnsAsync(_data.RoleInfoModels.Select(r => r.RoleName).ToList());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            var actual = await userService.GetByIdAsync(id);

            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.UserRoles));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserService_GetById_ReturnsCardIndexException(int id)
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetUsersUMAsync())
                .Throws(new Exception());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await userService.GetByIdAsync(id));
        }

        [TestCase("First@mail.com")]
        [TestCase("Second@mail.com")]
        public async Task UserService_GetByEmail_ReturnsUser(string email)
        {
            var expected = _data.UserInfoModels.First(u => u.Email == email);
            var fromDb = _data.Users.First(u => u.Email == email);
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(fromDb);
            mockUsersRolesManager
                .Setup(x => x.GetRolesFromUserManagerAsync(It.IsAny<User>()))
                .ReturnsAsync(_data.RoleInfoModels.Select(r => r.RoleName).ToList());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            var actual = await userService.GetByEmailAsync(email);

            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.UserRoles));
        }

        [TestCase("First@mail.com")]
        [TestCase("Second@mail.com")]
        public async Task UserService_GetById_ReturnsCardIndexException(string email)
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .Throws(new Exception());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await userService.GetByEmailAsync(email));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserService_ModifyUser_ChangesUser(int id)
        {
            var expected = _data.Users.First(u => u.Id == id);
            var givenModel = _data.UserInfoModels.First(u => u.Id == id);

            var newModel = new User()
            {
                Id = id,
                FirstName = "NewModel",
                LastName = "NewModel",
                DateOfBirth = new DateTime(1950, 1, 1),
                City = "NewCity",
                Email = "newnew@mail.com",
                UserName = "newnew@mail.com",
                PhoneNumber = "+11(111)1111111"
            };
            var existingRoles = new List<string> { "Unknown" };

            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetUsersUMAsync())
                .ReturnsAsync(new List<User> { newModel });
            mockUsersRolesManager
                .Setup(x => x.GetRolesFromUserManagerAsync(It.IsAny<User>()))
                .ReturnsAsync(existingRoles);
            mockUsersRolesManager
                .Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()));
            mockUsersRolesManager
                .Setup(x => x.AddUserToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            mockUsersRolesManager
                .Setup(x => x.UpdateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            await userService.ModifyUserAsync(givenModel);

            mockUsersRolesManager.Verify(x => x.RemoveFromRolesAsync(It.Is<User>(
                c => c.Id == id), It.Is<IEnumerable<string>>(
                c => c.First() == existingRoles.First())), Times.Once);

            mockUsersRolesManager.Verify(x => x.AddUserToRoleAsync(It.Is<User>(
                c => c.Id == givenModel.Id), It.Is<string>(
                c => c == givenModel.UserRoles.First())), Times.Once);

            mockUsersRolesManager.Verify(x => x.UpdateUserAsync(It.Is<User>(
                c => c.Id == givenModel.Id &&
                     c.FirstName == givenModel.FirstName &&
                     c.LastName == givenModel.LastName &&
                     c.DateOfBirth == givenModel.DateOfBirth &&
                     c.City == givenModel.City &&
                     c.Email == givenModel.Email &&
                     c.UserName == givenModel.Email &&
                     c.PhoneNumber == givenModel.Phone)), Times.Once);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserService_ModifyUser_ReturnsErrorResponse(int id)
        {
            var expected = _data.Users.First(u => u.Id == id);
            var givenModel = _data.UserInfoModels.First(u => u.Id == id);

            var newModel = new User()
            {
                Id = id,
                FirstName = "NewModel",
                LastName = "NewModel",
                DateOfBirth = new DateTime(1950, 1, 1),
                City = "NewCity",
                Email = "newnew@mail.com",
                UserName = "newnew@mail.com",
                PhoneNumber = "+11(111)1111111"
            };
            var existingRoles = new List<string> { "Unknown" };

            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetUsersUMAsync())
                .ReturnsAsync(new List<User> { newModel });
            mockUsersRolesManager
                .Setup(x => x.GetRolesFromUserManagerAsync(It.IsAny<User>()))
                .ReturnsAsync(existingRoles);
            mockUsersRolesManager
                .Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()));
            mockUsersRolesManager
                .Setup(x => x.AddUserToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            mockUsersRolesManager
                .Setup(x => x.UpdateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed(new []{new IdentityError()}));
            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            var result = await userService.ModifyUserAsync(givenModel);

            Assert.That(result.Succeeded,Is.False);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserService_ModifyUser_ReturnsCardIndexException(int id)
        {
            var model = _data.UserInfoModels.ToList()[id];
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetUsersUMAsync())
                .Throws(new Exception());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await userService.ModifyUserAsync(model));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserService_DeleteUser_DeletesUser(int id)
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetUsersUMAsync())
                .ReturnsAsync(_data.Users.ToList());
            mockUsersRolesManager
                .Setup(x => x.DeleteUserAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            var actual = await userService.DeleteUserAsync(id);

            mockUsersRolesManager.Verify(x => x.DeleteUserAsync(It.IsAny<User>()), Times.Once);

            Assert.That(actual.Succeeded, Is.EqualTo(true));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserService_DeleteUser_ReturnsErrorOnNull(int id)
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetUsersUMAsync())
                .ReturnsAsync(new List<User>());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            var actual = await userService.DeleteUserAsync(id);

            mockUsersRolesManager.Verify(x => x.DeleteUserAsync(It.IsAny<User>()), Times.Never);

            Assert.That(actual.Succeeded, Is.False);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserService_DeleteUser_ReturnsErrorResponse(int id)
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetUsersUMAsync())
                .ReturnsAsync(_data.Users.ToList());
            mockUsersRolesManager
                .Setup(x => x.DeleteUserAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed(new []{new IdentityError()}));

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            var actual = await userService.DeleteUserAsync(id);

            mockUsersRolesManager.Verify(x => x.DeleteUserAsync(It.IsAny<User>()), Times.Once);

            Assert.That(actual.Succeeded, Is.False);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserService_DeleteUser_ReturnsCardIndexException(int id)
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetUsersUMAsync())
                .Throws(new Exception());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await userService.DeleteUserAsync(id));
        }

        [Test]
        public async Task UserService_GetAllRoles_ReturnsAllRoles()
        {
            var expected = _data.RoleInfoModels;
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetRolesFromRoleManagerAsync())
                .ReturnsAsync(_data.Roles.ToList());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            var actual = await userService.GetAllRolesAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UserService_GetAllRoles_ReturnsCardIndexException()
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetRolesFromRoleManagerAsync())
                .Throws(new Exception());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await userService.GetAllRolesAsync());
        }

        [TestCase("Admin")]
        [TestCase("Moderator")]
        public async Task UserService_GetRoleByName_ReturnsRole(string roleName)
        {
            var expected = _data.RoleInfoModels.First(r => r.RoleName == roleName);
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetRolesFromRoleManagerAsync())
                .ReturnsAsync(_data.Roles.ToList());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            var actual = await userService.GetRoleByNameAsync(roleName);

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("Admin")]
        [TestCase("Moderator")]
        public async Task UserService_GetRoleByName_ReturnsCardIndexException(string roleName)
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetRolesFromRoleManagerAsync())
                .Throws(new Exception());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await userService.GetRoleByNameAsync(roleName));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserService_AddRole_AddsRole(int id)
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.CreateRoleAsync(It.IsAny<UserRole>()));

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);
            var roleToAdd = _data.RoleInfoModels.First(r => r.Id == id);

            await userService.AddRoleAsync(roleToAdd);

            mockUsersRolesManager.Verify(x => x.CreateRoleAsync(It.Is<UserRole>(
                c => c.Id == 0 &&
                   c.Name == roleToAdd.RoleName)), Times.Once);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserService_AddRole_ReturnsCardIndexException(int id)
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.CreateRoleAsync(It.IsAny<UserRole>()))
                .Throws(new Exception());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);
            var roleToAdd = _data.RoleInfoModels.First(r => r.Id == id);

            Assert.ThrowsAsync<CardIndexException>(async () => await userService.AddRoleAsync(roleToAdd));
        }

        [TestCase("Admin")]
        [TestCase("Moderator")]
        public async Task UserService_DeleteRole_DeletesRole(string roleName)
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetRolesFromRoleManagerAsync())
                .ReturnsAsync(_data.Roles.ToList());
            mockUsersRolesManager
                .Setup(x => x.DeleteRoleAsync(It.IsAny<UserRole>()))
                .ReturnsAsync(IdentityResult.Success);

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            await userService.DeleteRoleAsync(roleName);

            mockUsersRolesManager.Verify(x => x.DeleteRoleAsync(It.IsAny<UserRole>()), Times.Once);
        }

        [TestCase("Admin")]
        [TestCase("Moderator")]
        public async Task UserService_DeleteRole_ReturnsCardIndexException(string roleName)
        {
            var mockUsersRolesManager = new Mock<IManageUsersRoles>();
            mockUsersRolesManager
                .Setup(x => x.GetRolesFromRoleManagerAsync())
                .Throws(new Exception());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUsersRolesManager.Object);

            Assert.ThrowsAsync<CardIndexException>(async () => await userService.DeleteRoleAsync(roleName));
        }

        [Test]
        public async Task UserService_ChangeUserPasswordAsync_ChangesPassword()
        {
            var mockUserRolesManager = new Mock<IManageUsersRoles>();
            mockUserRolesManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(_data.Users.First());
            mockUserRolesManager
                .Setup(x => x.ChangeUserPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUserRolesManager.Object);
            var actual = await userService.ChangeUserPasswordAsync(_data.UserInfoModels.First(), "oldPass", "newPass");

            mockUserRolesManager.Verify(
                x => x.ChangeUserPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.That(actual.Succeeded, Is.True);
        }

        [Test]
        public async Task UserService_ChangeUserPasswordAsync_ReturnsErrorResponse()
        {
            var mockUserRolesManager = new Mock<IManageUsersRoles>();
            mockUserRolesManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(_data.Users.First());
            mockUserRolesManager
                .Setup(x => x.ChangeUserPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new []{new IdentityError()}));

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUserRolesManager.Object);
            var actual = await userService.ChangeUserPasswordAsync(_data.UserInfoModels.First(), "oldPass", "newPass");

            mockUserRolesManager.Verify(
                x => x.ChangeUserPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.That(actual.Succeeded, Is.False);
        }

        [Test]
        public async Task UserService_ChangeUserPasswordAsync_ThrowsCardIndexException()
        {
            var mockUserRolesManager = new Mock<IManageUsersRoles>();
            mockUserRolesManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUserRolesManager.Object);
            
            Assert.ThrowsAsync<CardIndexException>(async () =>
                await userService.ChangeUserPasswordAsync(_data.UserInfoModels.First(), "oldPass", "newPass"));
        }

        [Test]
        public async Task UserService_CheckPasswordAsync_ChecksPassword()
        {
            var mockUserRolesManager = new Mock<IManageUsersRoles>();
            mockUserRolesManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(_data.Users.First());
            mockUserRolesManager
                .Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUserRolesManager.Object);
            var actual = await userService.CheckPasswordAsync(_data.UserInfoModels.First(), "oldPass");

            mockUserRolesManager.Verify(
                x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            Assert.That(actual, Is.True);
        }

        [Test]
        public async Task UserService_CheckPasswordAsync_ThrowsCardIndexException()
        {
            var mockUserRolesManager = new Mock<IManageUsersRoles>();
            mockUserRolesManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var userService = new UserService(DbTestHelper.CreateMapperProfile(), mockUserRolesManager.Object);

            Assert.ThrowsAsync<CardIndexException>(async () =>
                await userService.CheckPasswordAsync(_data.UserInfoModels.First(), "oldPass"));
        }
    }
}
