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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CardIndexTests.WebApiTests
{
    [TestFixture]
    public class UserIntegrationTests
    {
        private CardIndexWebAppFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/user";
        private const string roleUri = "roles";

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

        #region GetAllTests

        [Test]
        public async Task UserController_GetAll_ReturnsAllUsers()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var expected = UserInfoModels.ToList();

            var httpResponse = await _client.GetAsync(RequestUri);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserInfoModel>>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected).Using(new UserInfoModelComparer()));
        }

        [Test]
        public async Task UserController_GetAll_FromEmptyTable_ReturnsNotFound()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync(RequestUri);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task UserController_GetAll_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region GetByIdTests

        [TestCase(1)]
        [TestCase(2)]
        public async Task UserController_GetById_ReturnsUser(int id)
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var expected = UserInfoModels.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{id}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserInfoModel>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected[id - 1]).Using(new UserInfoModelComparer()));
        }

        [TestCase(1234)]
        [TestCase(-1)]
        [TestCase(0)]
        public async Task UserController_GetById_ReturnsNotFound(int id)
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{id}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task UserController_GetById_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region UpdateTests

        [Test]
        public async Task UserController_UpdateExisting_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

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
            var httpResponse = await _client.PutAsync($"{RequestUri}/{User4.Id}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"{RequestUri}/{User4.Id}");
            getResponse.EnsureSuccessStatusCode();
            var stringResponse = await getResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserInfoModel>(stringResponse);

            Assert.That(actual, Is.EqualTo(User4).Using(new UserInfoModelComparer()));
        }

        [Test]
        public async Task UserController_Update_Unauthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

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
            var httpResponse = await _client.PutAsync($"{RequestUri}/{User4.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task UserController_UpdateExisting_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            UserInfoModel user4 = new UserInfoModel()
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

            var content = new StringContent(JsonConvert.SerializeObject(user4), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{user4.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UserController_Update_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();
            UserInfoModel user4 = new UserInfoModel()
            {
                Id = 5,
                FirstName = "",
                LastName = "Grishkov",
                Email = "newmail@gmail.com",
                City = "Rivne",
                DateOfBirth = new DateTime(2001, 01, 12),
                Phone = "+38(012)3456789",
                UserRoles = new List<string> { "Registered" }
            };
            var mockService = new Mock<IUserService>();
            var userController = new UserController(mockService.Object);
            userController.ModelState.AddModelError("FirstName", "FirstName is empty");

            var result = await userController.Update(5, user4);
            var objectResult = (BadRequestObjectResult)result.Result;
            var responseObject = objectResult.Value as Response;

            Assert.That(responseObject?.Succeeded, Is.False);
        }

        #endregion

        #region DeleteTests

        [TestCase(1, 2)]
        public async Task UserController_DeleteExisting_Success(int id, int newLength)
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.DeleteAsync($"{RequestUri}/{id}");
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync(RequestUri);
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserInfoModel>>(stringResponse);

            Assert.That(actual.Count(), Is.EqualTo(newLength));
        }

        [Test]
        public async Task UserController_Delete_Unauthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var httpResponse = await _client.DeleteAsync($"{RequestUri}/1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task UserController_DeleteExisting_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.DeleteAsync($"{RequestUri}/1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region GetAllRolesTests

        [Test]
        public async Task UserController_GetAllRoles_ReturnsAllRoles()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var expected = UserRoleInfoModels.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{roleUri}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserRoleInfoModel>>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected).Using(new UserRoleInfoModelComparer()));
        }

        [Test]
        public async Task UserController_GetAllRoles_ReturnsNotFound()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{roleUri}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task UserController_GetAllRoles_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{roleUri}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region GetRoleByNameTests

        [TestCase("Admin", 1)]
        [TestCase("Registered", 2)]
        public async Task UserController_GetRoleByName_ReturnsRole(string name, int id)
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var expected = UserRoleInfoModels.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{roleUri}/{name}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserRoleInfoModel>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected[id - 1]).Using(new UserRoleInfoModelComparer()));
        }

        [TestCase("NonExistent")]
        public async Task UserController_GetRoleByName_ReturnsNotFound(string name)
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{roleUri}/{name}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task UserController_GetRoleByName_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{roleUri}/someRole");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region AddRoleTests

        [Test]
        public async Task UserController_AddNewRole_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var newRole = new UserRoleInfoModel
            {
                Id = 4,
                RoleName = "NewAuthor"
            };
            var expectedLength = UserRoleInfoModels.Count() + 1;

            var content = new StringContent(JsonConvert.SerializeObject(newRole), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/{roleUri}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync($"{RequestUri}/{roleUri}");
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserRoleInfoModel>>(stringResponse);
            var added = actual.FirstOrDefault(r => r.Id == newRole.Id);

            Assert.That(actual.Count(), Is.EqualTo(expectedLength));
            Assert.That(added, Is.EqualTo(newRole).Using(new UserRoleInfoModelComparer()));
        }

        [Test]
        public async Task UserController_AddNewRole_NullModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);
                });
            }).CreateClient();

            UserRoleInfoModel role4 = null as UserRoleInfoModel;

            var content = new StringContent(JsonConvert.SerializeObject(role4), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/{roleUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UserController_AddNew_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();
            var newRole = new UserRoleInfoModel
            {
                Id = 4,
                RoleName = ""
            };
            var mockService = new Mock<IUserService>();
            var userController = new UserController(mockService.Object);
            userController.ModelState.AddModelError("RoleName", "RoleName is empty");

            var result = await userController.AddRole(newRole);
            var objectResult = (BadRequestObjectResult)result.Result;
            var responseObject = objectResult.Value as Response;

            Assert.That(responseObject?.Succeeded, Is.False);
        }

        [Test]
        public async Task UserController_AddNewRole_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var newRole = new UserRoleInfoModel
            {
                Id = 4,
                RoleName = "newRole"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newRole), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/{roleUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region DeleteRoleTests

        [TestCase("Moderator", 2)]
        public async Task UserController_DeleteExistingRole_Success(string name, int newLength)
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.DeleteAsync($"{RequestUri}/{roleUri}/{name}");
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync($"{RequestUri}/{roleUri}");
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserRoleInfoModel>>(stringResponse);

            Assert.That(actual.Count(), Is.EqualTo(newLength));
        }

        [TestCase("##")]
        public async Task UserController_DeleteExistingRole_BadRequest(string name)
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var expectedLength = UserRoleInfoModels.Count();
            var httpResponse = await _client.DeleteAsync($"{RequestUri}/{roleUri}/{name}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UserController_DeleteExistingRole_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.DeleteAsync($"{RequestUri}/{roleUri}/someRole");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region GetUserCabinetTests

        [Test]
        public async Task UserController_GetUserCabinet_ReturnsUser()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var expected = UserInfoModels.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}/cabinet");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserInfoModel>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected[0]).Using(new UserInfoModelComparer()));
        }

        [Test]
        public async Task UserController_GetUserCabinet_ReturnsNotFound()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/cabinet");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task UserController_GetUserCabinet_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/cabinet");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region ModifyUserCabinetTests

        [Test]
        public async Task UserController_ModifyUserCabinet_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            //id and roles not updated in cabinet, they set like in expected
            UserInfoModel user4 = new UserInfoModel()
            {
                Id = 1,
                FirstName = "newName",
                LastName = "newLastName",
                Email = "newmail@mail.com",
                City = "Odesa",
                DateOfBirth = new DateTime(2000, 01, 12),
                Phone = "+38(012)3436789",
                UserRoles = new List<string> { "Admin" }
            };

            var content = new StringContent(JsonConvert.SerializeObject(user4), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/cabinet/modify", content);
            httpResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"{RequestUri}/{user4.Id}");
            getResponse.EnsureSuccessStatusCode();
            var stringResponse = await getResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserInfoModel>(stringResponse);

            Assert.That(actual, Is.EqualTo(user4).Using(new UserInfoModelComparer()));
        }

        [Test]
        public async Task UserController_ModifyUserCabinet_Unauthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            UserInfoModel user4 = new UserInfoModel()
            {
                Id = 1,
                FirstName = "newName",
                LastName = "Grishkov",
                Email = "newmail@gmail.com",
                City = "Rivne",
                DateOfBirth = new DateTime(2001, 01, 12),
                Phone = "+38(012)3456789",
                UserRoles = new List<string> { "Registered" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(user4), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{user4.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task UserController_ModifyUserCabinet_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            UserInfoModel user4 = new UserInfoModel()
            {
                Id = 1,
                FirstName = "newName",
                LastName = "Grishkov",
                Email = "newmail@gmail.com",
                City = "Rivne",
                DateOfBirth = new DateTime(2001, 01, 12),
                Phone = "+38(012)3456789",
                UserRoles = new List<string> { "Registered" }
            };

            var content = new StringContent(JsonConvert.SerializeObject(user4), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/cabinet/modify", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UserController_ModifyUserCabinet_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();
            UserInfoModel user4 = new UserInfoModel()
            {
                Id = 1,
                FirstName = "",
                LastName = "Grishkov",
                Email = "newmail@gmail.com",
                City = "Rivne",
                DateOfBirth = new DateTime(2001, 01, 12),
                Phone = "+38(012)3456789",
                UserRoles = new List<string> { "Registered" }
            };
            var mockService = new Mock<IUserService>();
            var userController = new UserController(mockService.Object);
            userController.ModelState.AddModelError("FirstName", "FirstName is empty");

            var result = await userController.ModifyUserCabinet(user4);
            var objectResult = (BadRequestObjectResult)result.Result;
            var responseObject = objectResult.Value as Response;

            Assert.That(responseObject?.Succeeded, Is.False);
        }

        #endregion

        #region ChangeUserPasswordTests

        [Test]
        public async Task UserController_ChangeUserPassword_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            ChangePasswordModel changePass = new ChangePasswordModel()
            {
                CurrentPassword = "_Aq12345678",
                NewPassword = "+Aq12345678",
                ConfirmNewPassword = "+Aq12345678"
            };

            var content = new StringContent(JsonConvert.SerializeObject(changePass), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/cabinet/changepass", content);
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<Response>(stringResponse);

            Assert.That(actual.Succeeded, Is.True);
        }

        [Test]
        public async Task UserController_ChangeUserPassword_NewPassNotValid()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            ChangePasswordModel changePass = new ChangePasswordModel()
            {
                CurrentPassword = "_Aq12345678",
                NewPassword = "12345678",
                ConfirmNewPassword = "12345678"
            };

            var content = new StringContent(JsonConvert.SerializeObject(changePass), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/cabinet/changepass", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UserController_ChangeUserPassword_OldPassNotValid()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            ChangePasswordModel changePass = new ChangePasswordModel()
            {
                CurrentPassword = "12345678",
                NewPassword = "12345678",
                ConfirmNewPassword = "12345678"
            };

            var content = new StringContent(JsonConvert.SerializeObject(changePass), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/cabinet/changepass", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UserController_ChangeUserPassword_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            ChangePasswordModel changePass = new ChangePasswordModel()
            {
                CurrentPassword = "12345678",
                NewPassword = "12345678",
                ConfirmNewPassword = "12345678"
            };

            var content = new StringContent(JsonConvert.SerializeObject(changePass), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/cabinet/changepass", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UserController_ChangeUserPassword_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();
            ChangePasswordModel changePass = new ChangePasswordModel()
            {
                CurrentPassword = "12345678",
                NewPassword = "12345678",
                ConfirmNewPassword = "notConfirmed"
            };
            var mockService = new Mock<IUserService>();
            var userController = new UserController(mockService.Object);
            userController.ModelState.AddModelError("ConfirmNewPassword", "Passwords do not match");

            var result = await userController.ChangeUserPassword(changePass);
            var objectResult = (BadRequestObjectResult)result.Result;
            var responseObject = objectResult.Value as Response;

            Assert.That(responseObject?.Succeeded, Is.False);
        }

        #endregion

    }
}
