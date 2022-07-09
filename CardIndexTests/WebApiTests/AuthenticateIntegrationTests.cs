using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using card_index_Web_API.Controllers;
using CardIndexTests.Helpers;
using CardIndexTests.WebApiTests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CardIndexTests.WebApiTests
{
    [TestFixture]
    public class AuthenticateIntegrationTests
    {
        private CardIndexWebAppFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/authenticate";

        private IEnumerable<UserInfoModel> UserInfoModels = new List<UserInfoModel>
        {
            new UserInfoModel()
            {
                Id = 1,
                FirstName = "Oleksandr",
                LastName = "Shyman",
                Email = "mymail@gmail.com",
                City = "Rivne",
                DateOfBirth = new DateTime(1988, 12, 12),
                Phone = "+38(012)3456789",
                UserRoles = new List<string> { "Admin" }
            },
            new UserInfoModel()
            {
                Id = 2,
                FirstName = "Aleksey",
                LastName = "Grishkov",
                Email = "newmail@gmail.com",
                City = "Rivne",
                DateOfBirth = new DateTime(2001, 01, 12),
                Phone = "+38(012)3456789",
                UserRoles = new List<string> { "Registered" }
            },
            new UserInfoModel()
            {
                Id = 3,
                FirstName = "Taras",
                LastName = "Bobkin",
                Email = "taras@gmail.com",
                City = "Rivne",
                DateOfBirth = new DateTime(1976, 01, 12),
                Phone = "+38(012)3456789",
                UserRoles = new List<string> { "Moderator" }
            }
        };

        private IEnumerable<UserRoleInfoModel> UserRoleInfoModels = new List<UserRoleInfoModel>()
        {
            new UserRoleInfoModel()
            {
                Id = 1,
                RoleName = "Admin"
            },
            new UserRoleInfoModel()
            {
                Id = 2,
                RoleName = "Registered"
            },
            new UserRoleInfoModel()
            {
                Id = 3,
                RoleName = "Moderator"
            },
        };

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        #region RegisterTests

        [Test]
        public async Task AuthenticateController_Registration_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                })).CreateClient();

            UserRegistrationModel newUser = new UserRegistrationModel
            {
                FirstName = "newName",
                LastName = "newLastName",
                Email = "unused@mail.com",
                Password = "_Aq12345678",
                ConfirmPassword = "_Aq12345678"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/register", content);
            httpResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"api/user");
            getResponse.EnsureSuccessStatusCode();
            var stringResponse = await getResponse.Content.ReadAsStringAsync();
            var allUsersAfterRegister = JsonConvert.DeserializeObject<IEnumerable<UserInfoModel>>(stringResponse);
            var actual = allUsersAfterRegister.FirstOrDefault(u => u.Email == newUser.Email);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.FirstName, Is.EqualTo(newUser.FirstName));
            Assert.That(actual.LastName, Is.EqualTo(newUser.LastName));
            Assert.That(actual.Email, Is.EqualTo(newUser.Email));
            Assert.That(actual.Phone, Is.EqualTo(newUser.Phone));
        }

        [Test]
        public async Task AuthenticateController_Registration_Conflict()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                })).CreateClient();

            UserRegistrationModel newUser = new UserRegistrationModel
            {
                FirstName = "newName",
                LastName = "newLastName",
                Email = "mymail@gmail.com", //user with this email already exists
                Password = "_Aq12345678",
                ConfirmPassword = "_Aq12345678"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/register", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }

        [Test]
        public async Task AuthenticateController_Registration_NullModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);
                })).CreateClient();

            UserRegistrationModel newUser = null as UserRegistrationModel;

            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/register", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task AuthenticateController_Registration_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            UserRegistrationModel newUser = new UserRegistrationModel
            {
                FirstName = "",
                LastName = "newLastName",
                Email = "unused@mail.com",
                Password = "_Aq12345678",
                ConfirmPassword = "_Aq12345678"
            };

            var mockService = new Mock<IAuthenticationService>();
            var authenticateController = new AuthenticateController(mockService.Object);
            authenticateController.ModelState.AddModelError("FirstName", "FirstName is empty");

            var result = await authenticateController.Register(newUser);
            var objectResult = (BadRequestObjectResult)result.Result;
            var responseObject = objectResult.Value as Response;

            Assert.That(responseObject?.Succeeded, Is.False);
        }

        [Test]
        public async Task AuthenticateController_Registration_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IAuthenticationService, FakeAuthenticationService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            UserRegistrationModel newUser = new UserRegistrationModel
            {
                FirstName = "newName",
                LastName = "newLastName",
                Email = "unused@mail.com",
                Password = "_Aq12345678",
                ConfirmPassword = "_Aq12345678"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/register", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region LoginTests

        [Test]
        public async Task AuthenticateController_Login_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.CreateClient();

            UserLoginModel user = new UserLoginModel
            {
                Email = "mymail@gmail.com",
                Password = "_Aq12345678"
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/login", content);
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<LoginResponse>(stringResponse);

            Assert.That(actual.Succeeded, Is.True);
            Assert.That(actual.Token, Is.Not.Null);
        }

        [Test]
        public async Task AuthenticateController_Login_Unauthorized()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.CreateClient();

            UserLoginModel user = new UserLoginModel
            {
                Email = "mymail@gmail.com",
                Password = "_A678"
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/login", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task AuthenticateController_Login_NullModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);
                })).CreateClient();

            UserLoginModel user = null as UserLoginModel;

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/login", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task AuthenticateController_Login_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            UserLoginModel user = new UserLoginModel
            {
                Email = "",
                Password = "_Aq12345678"
            };

            var mockService = new Mock<IAuthenticationService>();
            var authenticateController = new AuthenticateController(mockService.Object);
            authenticateController.ModelState.AddModelError("Email", "Email is empty");

            var result = await authenticateController.Login(user);
            var objectResult = (UnauthorizedObjectResult)result.Result;
            var responseObject = objectResult.Value as LoginResponse;

            Assert.That(responseObject?.Succeeded, Is.False);
        }

        [Test]
        public async Task AuthenticateController_Login_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IAuthenticationService, FakeAuthenticationService>();
                });
            }).CreateClient();

            UserLoginModel user = new UserLoginModel
            {
                Email = "mymail@gmail.com",
                Password = "_Aq12345678"
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/login", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region LogoutTests

        [Test]
        public async Task AuthenticateController_Logout_Works()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var mockService = new Mock<IAuthenticationService>();
            var authenticateController = new AuthenticateController(mockService.Object);

            await authenticateController.LogOut();

            mockService.Verify(x=>x.LogOutAsync(), Times.Once);
        }

        #endregion
    }
}
