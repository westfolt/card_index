using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Models;
using card_index_BLL.Security;
using card_index_BLL.Services;
using card_index_DAL.Entities;
using CardIndexTests.BllTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardIndexTests.BllTests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private readonly DataForEntityTests _data = DataForEntityTests.GetTestData();
        private JwtHandler _jwtHandler;
        private Mock<IManageUsersRoles> _usersRolesManager;

        [SetUp]
        public void Initialize()
        {
            _usersRolesManager = new Mock<IManageUsersRoles>();

            Mock<IConfigurationSection> jwtSection = new Mock<IConfigurationSection>();
            jwtSection
                .Setup(x => x.GetSection("securityKey").Value)
                .Returns("CardApi_SecretKey");
            jwtSection
                .Setup(x => x.GetSection("validIssuer").Value)
                .Returns("https://localhost:5001");
            jwtSection
                .Setup(x => x.GetSection("validAudience").Value)
                .Returns("https://localhost:5001");
            jwtSection
                .Setup(x => x.GetSection("expiryInMinutes").Value)
                .Returns("35");

            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig
                .Setup(x => x.GetSection("JwtSettings"))
                .Returns(jwtSection.Object);

            _jwtHandler = new JwtHandler(mockConfig.Object, _usersRolesManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _jwtHandler = null;
            _usersRolesManager = null;
        }

        [Test]
        public async Task AuthenticationService_LoginUser_Success()
        {
            var userFromDb = _data.Users.First();
            _usersRolesManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(userFromDb);
            _usersRolesManager
                .Setup(x => x.GetRolesFromUserManagerAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { "Admin" });
            _usersRolesManager
                .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            var authenticationService = new AuthenticationService(_usersRolesManager.Object, _jwtHandler);

            var result = await authenticationService.LoginUserAsync(new UserLoginModel
            { Email = "mymail@gmail.com", Password = "mypass123" });

            Assert.That(result.Succeeded, Is.True);
        }

        [Test]
        public async Task AuthenticationService_LoginUser_ReturnsErrorOnNull()
        {
            var userFromDb = _data.Users.First();
            _usersRolesManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(userFromDb);
            _usersRolesManager
                .Setup(x => x.GetRolesFromUserManagerAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { "Admin" });
            _usersRolesManager
                .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            var authenticationService = new AuthenticationService(_usersRolesManager.Object, _jwtHandler);

            var result = await authenticationService.LoginUserAsync(new UserLoginModel
            { Email = "mymail@gmail.com", Password = "mypass123" });

            Assert.That(result.Succeeded, Is.False);
        }

        [Test]
        public async Task AuthenticationService_LoginUser_ReturnsErrorOnWrongLogin()
        {
            _usersRolesManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(null as User);

            var authenticationService = new AuthenticationService(_usersRolesManager.Object, _jwtHandler);

            var result = await authenticationService.LoginUserAsync(new UserLoginModel
            { Email = "mymail@gmail.com", Password = "mypass123" });

            Assert.That(result.Succeeded, Is.False);
        }

        [Test]
        public async Task AuthenticationService_LoginUser_ThrowsException()
        {
            _usersRolesManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var authenticationService = new AuthenticationService(_usersRolesManager.Object, _jwtHandler);

            Assert.ThrowsAsync<CardIndexException>(async () => await authenticationService.LoginUserAsync(new UserLoginModel
            { Email = "mymail@gmail.com", Password = "mypass123" }));
        }

        [Test]
        public async Task AuthenticationService_RegisterUser_Success()
        {
            var userFromDb = _data.Users.First();
            var registerModel = new UserRegistrationModel
            {
                FirstName = userFromDb.FirstName,
                LastName = userFromDb.LastName,
                Email = "nonexisting@mail.com",
                Phone = userFromDb.PhoneNumber,
                Password = "mypass123",
                ConfirmPassword = "mypass123"
            };

            _usersRolesManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(null as User);
            _usersRolesManager
                .Setup(x => x.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var authenticationService = new AuthenticationService(_usersRolesManager.Object, _jwtHandler);
            var result = await authenticationService.RegisterUserAsync(registerModel);

            _usersRolesManager.Verify(x => x.CreateUserAsync(It.Is<User>(u =>
                u.FirstName == registerModel.FirstName &&
                u.LastName == registerModel.LastName &&
                u.Email == registerModel.Email &&
                u.UserName == registerModel.Email &&
                u.PhoneNumber == registerModel.Phone), It.Is<string>(p =>
                p == registerModel.Password &&
                p == registerModel.ConfirmPassword)), Times.Once);

            Assert.That(result.Succeeded, Is.True);

        }

        [Test]
        public async Task AuthenticationService_RegisterUser_ErrorOnExisting()
        {
            _usersRolesManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(_data.Users.First());

            var authenticationService = new AuthenticationService(_usersRolesManager.Object, _jwtHandler);
            var result = await authenticationService.RegisterUserAsync(new UserRegistrationModel { Email = "newmail@mail.com" });

            Assert.That(result.Succeeded, Is.False);
        }

        [Test]
        public async Task AuthenticationService_RegisterUser_ErrorOnFailure()
        {
            var userFromDb = _data.Users.First();
            var registerModel = new UserRegistrationModel
            {
                FirstName = userFromDb.FirstName,
                LastName = userFromDb.LastName,
                Email = "nonexisting@mail.com",
                Phone = userFromDb.PhoneNumber,
                Password = "mypass123",
                ConfirmPassword = "mypass123"
            };

            _usersRolesManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(null as User);
            _usersRolesManager
                .Setup(x => x.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new[] { new IdentityError() }));

            var authenticationService = new AuthenticationService(_usersRolesManager.Object, _jwtHandler);
            var result = await authenticationService.RegisterUserAsync(registerModel);

            Assert.That(result.Succeeded, Is.False);
        }

        [Test]
        public async Task AuthenticationService_RegisterUser_ThrowsCardIndexException()
        {
            var userFromDb = _data.Users.First();
            var registerModel = new UserRegistrationModel
            {
                FirstName = userFromDb.FirstName,
                LastName = userFromDb.LastName,
                Email = "nonexisting@mail.com",
                Phone = userFromDb.PhoneNumber,
                Password = "mypass123",
                ConfirmPassword = "mypass123"
            };

            _usersRolesManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(null as User);
            _usersRolesManager
                .Setup(x => x.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var authenticationService = new AuthenticationService(_usersRolesManager.Object, _jwtHandler);

            Assert.ThrowsAsync<CardIndexException>(async () => await authenticationService.RegisterUserAsync(registerModel));
        }

        [Test]
        public async Task AuthenticationService_Logout_Success()
        {
            var authenticationService = new AuthenticationService(_usersRolesManager.Object, _jwtHandler);
            await authenticationService.LogOutAsync();

            _usersRolesManager.Verify(x => x.SignOutAsync(), Times.Once);
        }
    }
}
