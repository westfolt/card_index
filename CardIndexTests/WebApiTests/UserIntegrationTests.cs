using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Models.Identity.Models;
using CardIndexTests.Helpers;
using CardIndexTests.WebApiTests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CardIndexTests.WebApiTests
{
    public abstract class UserIntegrationTests
    {
        protected CardIndexWebAppFactory _factory;
        protected HttpClient _client;
        protected const string RequestUri = "api/user/";
        protected const string roleUri = "roles";

        protected IEnumerable<UserInfoModel> UserInfoModels = new List<UserInfoModel>
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

        protected IEnumerable<UserRoleInfoModel> UserRoleInfoModels = new List<UserRoleInfoModel>()
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
    }

    [TestFixture]
    public class UserIntegrationSuccessTestsWithAuthentication : UserIntegrationTests
    {
        [SetUp]
        public void Initialize()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        [Test]
        public async Task UserController_GetAll_ReturnsAllUsers()
        {
            var expected = UserInfoModels.ToList();

            var httpResponse = await _client.GetAsync(RequestUri);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserInfoModel>>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected).Using(new UserComparer()));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserController_GetById_ReturnsUser(int id)
        {
            var expected = UserInfoModels.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}{id}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserInfoModel>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected[id - 1]).Using(new UserComparer()));
        }

        [Test]
        public async Task UserController_UpdateExisting_Success()
        {
            UserInfoModel User4 = new UserInfoModel()
            {
                Id = 2,
                FirstName = "newName",
                LastName = "newLastName",
                Email = "newmail@mail.com",
                City = "Odesa",
                DateOfBirth = new DateTime(2000, 01, 12),
                Phone = "+38(012)3436789",
                UserRoles = new List<string> { "Moderator" }
            };

            var content = new StringContent(JsonConvert.SerializeObject(User4), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}{User4.Id}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"{RequestUri}{User4.Id}");
            getResponse.EnsureSuccessStatusCode();
            var stringResponse = await getResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserInfoModel>(stringResponse);

            Assert.That(actual, Is.EqualTo(User4).Using(new UserComparer()));
        }

        [TestCase(1, 2)]
        public async Task UserController_DeleteExisting_Success(int id, int newLength)
        {
            var httpResponse = await _client.DeleteAsync($"{RequestUri}{id}");
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync(RequestUri);
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserInfoModel>>(stringResponse);

            Assert.That(actual.Count(), Is.EqualTo(newLength));
        }

        [Test]
        public async Task UserController_GetAllRoles_ReturnsAllRoles()
        {
            var expected = UserRoleInfoModels.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}{roleUri}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserRoleInfoModel>>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected).Using(new UserRoleComparer()));
        }

        [TestCase("Admin", 1)]
        [TestCase("Registered", 2)]
        public async Task UserController_GetRoleByName_ReturnsRole(string name, int id)
        {
            var expected = UserRoleInfoModels.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}{roleUri}/{name}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserRoleInfoModel>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected[id - 1]).Using(new UserRoleComparer()));
        }

        [Test]
        public async Task UserController_AddNewRole_Success()
        {
            var newRole = new UserRoleInfoModel
            {
                Id = 4,
                RoleName = "NewAuthor"
            };
            var expectedLength = UserRoleInfoModels.Count() + 1;

            var content = new StringContent(JsonConvert.SerializeObject(newRole), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}{roleUri}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync($"{RequestUri}{roleUri}");
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserRoleInfoModel>>(stringResponse);
            var added = actual.FirstOrDefault(r => r.Id == newRole.Id);

            Assert.That(actual.Count(), Is.EqualTo(expectedLength));
            Assert.That(added, Is.EqualTo(newRole).Using(new UserRoleComparer()));
        }

        [TestCase("Moderator", 2)]
        public async Task UserController_DeleteExistingRole_Success(string name, int newLength)
        {
            var httpResponse = await _client.DeleteAsync($"{RequestUri}{roleUri}/{name}");
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync($"{RequestUri}{roleUri}");
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserRoleInfoModel>>(stringResponse);

            Assert.That(actual.Count(), Is.EqualTo(newLength));
        }
    }

    [TestFixture]
    public class UserIntegrationErrorTestsWithAuthorization : UserIntegrationTests
    {
        [SetUp]
        public void Initialize()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        //error tests, needing authorization
        [Test]
        public async Task UserController_GetAll_FromEmptyTable_ReturnsNotFound()
        {
            var httpResponse = await _client.GetAsync(RequestUri);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase(1234)]
        [TestCase(-1)]
        [TestCase(0)]
        public async Task UserController_GetById_ReturnsNotFound(int id)
        {
            var httpResponse = await _client.GetAsync($"{RequestUri}{id}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task UserController_GetAllRoles_ReturnsNotFound()
        {
            var httpResponse = await _client.GetAsync($"{RequestUri}{roleUri}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("NonExistent")]
        public async Task UserController_GetRoleByName_ReturnsNotFound(string name)
        {
            var httpResponse = await _client.GetAsync($"{RequestUri}{roleUri}/{name}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task UserController_AddNewRole_BadRequest()
        {
            var newRole = new UserRoleInfoModel
            {
                Id = 5,
                RoleName = "##"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newRole), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}{roleUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("##")]
        public async Task UserController_DeleteExistingRole_BadRequest(string name)
        {
            var expectedLength = UserRoleInfoModels.Count();
            var httpResponse = await _client.DeleteAsync($"{RequestUri}{roleUri}/{name}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }

    [TestFixture]
    public class UserIntegrationErrorTests : UserIntegrationTests
    {
        [SetUp]
        public void Initialize()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        
        

        [Test]
        public async Task UserController_Update_Unauthorized()
        {
            UserInfoModel User4 = new UserInfoModel()
            {
                Id = 5,
                FirstName = "newName",
                LastName = "Grishkov",
                Email = "newmail@gmail.com",
                City = "Rivne",
                DateOfBirth = new DateTime(2001, 01, 12),
                Phone = "+38(012)3456789",
                UserRoles = new List<string> { "Registered" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(User4), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}{User4.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task UserController_Delete_Unauthorized()
        {
            var User4 = new UserInfoModel
            {
                Id = 5,
                FirstName = "Aleksey",
                LastName = "Grishkov",
                Email = "newmail@gmail.com",
                City = "Rivne",
                DateOfBirth = new DateTime(2001, 01, 12),
                Phone = "+38(012)3456789",
                UserRoles = new List<string> { "Registered" }
            };
            var httpResponse = await _client.DeleteAsync($"{RequestUri}{User4.Id}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
