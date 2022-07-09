using card_index_BLL.Interfaces;
using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_Web_API.Controllers;
using CardIndexTests.Helpers;
using CardIndexTests.WebApiTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CardIndexTests.WebApiTests
{
    public class AuthorIntegrationTests
    {
        protected CardIndexWebAppFactory _factory;
        protected HttpClient _client;
        protected const string RequestUri = "api/author";

        protected IEnumerable<AuthorDto> AuthorDtos =
            new List<AuthorDto>()
            {
                new AuthorDto { Id = 1, FirstName = "James", LastName = "Benton", YearOfBirth = 1956, TextCardIds = new List<int>{1}},
                new AuthorDto { Id = 2, FirstName = "Donette", LastName = "Foller", YearOfBirth = 1989, TextCardIds = new List<int>{2} },
                new AuthorDto { Id = 3, FirstName = "Veronika", LastName = "Donald", YearOfBirth = 1990, TextCardIds = new List<int>{3}},
                new AuthorDto { Id = 4, FirstName = "Jack", LastName = "Wieser", YearOfBirth = 2000, TextCardIds = new List<int>{4} },
                new AuthorDto { Id = 5, FirstName = "Arnold", LastName = "Clark", YearOfBirth = 2001, TextCardIds = new List<int>{5} }
            };

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        #region GetAllWithPagingTests

        [Test]
        public async Task AuthorController_GetAllWithPaging_ReturnsAllAuthors()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.CreateClient();

            var expected = AuthorDtos.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<AuthorDto>>(stringResponse);

            Assert.That(actual.Data, Is.EqualTo(expected).Using(new AuthorDtoComparer()));
        }

        [Test]
        public async Task AuthorController_GetAllWithPaging_FromEmptyTable_ReturnsNotFound()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task AuthorController_GetAllWithPaging_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IAuthorService, FakeAuthorService>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region GetAllTests

        [Test]
        public async Task AuthorController_GetAll_ReturnsAllAuthors()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var expected = AuthorDtos.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}/all");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<AuthorDto>>(stringResponse);

            actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.TextCardIds));
            //Assert.That(actual, Is.EqualTo(expected).Using(new AuthorDtoComparer()));
        }

        [Test]
        public async Task AuthorController_GetAll_FromEmptyTable_ReturnsNotFound()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/all");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task AuthorController_GetAll_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IAuthorService, FakeAuthorService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/all");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region GetByIdTests

        [TestCase(1)]
        [TestCase(2)]
        public async Task AuthorController_GetById_ReturnsAuthor(int id)
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.CreateClient();

            var expected = AuthorDtos.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{id}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<AuthorDto>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected[id - 1]).Using(new AuthorDtoComparer()));
        }

        [TestCase(1234)]
        [TestCase(-1)]
        [TestCase(0)]
        public async Task AuthorController_GetById_ReturnsNotFound(int id)
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{id}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task AuthorController_GetById_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IAuthorService, FakeAuthorService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region AddTests

        [Test]
        public async Task AuthorController_AddNew_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var author6 = new AuthorDto
            {
                Id = 6,
                FirstName = "Author6",
                LastName = "Author6",
                YearOfBirth = 2001,
                TextCardIds = new List<int>()
            };
            var expectedLength = AuthorDtos.Count() + 1;

            var content = new StringContent(JsonConvert.SerializeObject(author6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<AuthorDto>>(stringResponse);
            var added = actual.Data.FirstOrDefault(a => a.Id == author6.Id);

            Assert.That(actual.Data.Count(), Is.EqualTo(expectedLength));
            Assert.That(added, Is.EqualTo(author6).Using(new AuthorDtoComparer()));
        }

        [Test]
        public async Task AuthorController_AddNew_UnAuthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var author6 = new AuthorDto
            {
                Id = 5,
                FirstName = "Author6",
                LastName = "Author6",
                YearOfBirth = 2001,
                TextCardIds = new List<int>()
            };
            var content = new StringContent(JsonConvert.SerializeObject(author6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task AuthorController_AddNew_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IAuthorService, FakeAuthorService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var author6 = new AuthorDto
            {
                Id = 5,
                FirstName = "Author6",
                LastName = "Author6",
                YearOfBirth = 2001,
                TextCardIds = new List<int>()
            };

            var content = new StringContent(JsonConvert.SerializeObject(author6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task AuthorController_AddNew_NullModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IAuthorService, FakeAuthorService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);
                });
            }).CreateClient();

            AuthorDto author6 = null as AuthorDto;

            var content = new StringContent(JsonConvert.SerializeObject(author6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task AuthorController_AddNew_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();
            var authorToAdd = new AuthorDto
            {
                Id = 5,
                FirstName = "",
                LastName = "Author6",
                YearOfBirth = 2001,
                TextCardIds = new List<int>()
            };
            var mockService = new Mock<IAuthorService>();
            var genreController = new AuthorController(mockService.Object);
            genreController.ModelState.AddModelError("FirstName", "FirstName is empty");

            var result = await genreController.Add(authorToAdd);
            var objectResult = (BadRequestObjectResult)result.Result;
            var responseObject = objectResult.Value as Response;

            Assert.That(responseObject?.Succeeded, Is.False);
        }

        #endregion

        #region UpdateTests

        [Test]
        public async Task AuthorController_UpdateExisting_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var author6 = new AuthorDto
            {
                Id = 5,
                FirstName = "Author6",
                LastName = "Author6",
                YearOfBirth = 2001,
                TextCardIds = new List<int> { 5 }
            };

            var content = new StringContent(JsonConvert.SerializeObject(author6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{author6.Id}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"{RequestUri}/{author6.Id}");
            getResponse.EnsureSuccessStatusCode();
            var stringResponse = await getResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<AuthorDto>(stringResponse);

            Assert.That(actual, Is.EqualTo(author6).Using(new AuthorDtoComparer()));
        }

        [Test]
        public async Task AuthorController_Update_UnAuthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var author6 = new AuthorDto
            {
                Id = 5,
                FirstName = "Author6",
                LastName = "Author6",
                YearOfBirth = 2001,
                TextCardIds = new List<int>()
            };
            var content = new StringContent(JsonConvert.SerializeObject(author6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{author6.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task AuthorController_UpdateExisting_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IAuthorService, FakeAuthorService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var author6 = new AuthorDto
            {
                Id = 5,
                FirstName = "Author6",
                LastName = "Author6",
                YearOfBirth = 2001,
                TextCardIds = new List<int>()
            };

            var content = new StringContent(JsonConvert.SerializeObject(author6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{author6.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task AuthorController_Update_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();
            var authorToAdd = new AuthorDto
            {
                Id = 5,
                FirstName = "Author6",
                LastName = "Author6",
                YearOfBirth = 2001,
                TextCardIds = new List<int>()
            };
            var mockService = new Mock<IAuthorService>();
            var genreController = new AuthorController(mockService.Object);
            genreController.ModelState.AddModelError("Title", "Title is empty");

            var result = await genreController.Update(5, authorToAdd);
            var objectResult = (BadRequestObjectResult)result.Result;
            var responseObject = objectResult.Value as Response;

            Assert.That(responseObject?.Succeeded, Is.False);
        }

        #endregion

        #region DeleteTests

        [TestCase(1, 4)]
        public async Task AuthorController_DeleteExisting_Success(int id, int newLength)
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

            var getAllResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<AuthorDto>>(stringResponse);

            Assert.That(actual.Data.Count(), Is.EqualTo(newLength));
        }

        [Test]
        public async Task AuthorController_Delete_UnAuthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var author6 = new AuthorDto
            {
                Id = 5,
                FirstName = "Author6",
                LastName = "Author6",
                YearOfBirth = 2001,
                TextCardIds = new List<int>()
            };
            var httpResponse = await _client.DeleteAsync($"{RequestUri}/{author6.Id}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task AuthorController_DeleteExisting_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IAuthorService, FakeAuthorService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.DeleteAsync($"{RequestUri}/1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion
    }
}
