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
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace CardIndexTests.WebApiTests
{
    [TestFixture]
    public class GenreIntegrationTests
    {
        private CardIndexWebAppFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/genre";

        private IEnumerable<GenreDto> GenreDtos = new List<GenreDto>
        {
            new GenreDto { Id = 1, Title = "Genre1", TextCardIds = new List<int>{1} },
            new GenreDto { Id = 2, Title = "Genre2", TextCardIds = new List<int>{2} },
            new GenreDto { Id = 3, Title = "Genre3", TextCardIds = new List<int>{3} },
            new GenreDto { Id = 4, Title = "Genre4", TextCardIds = new List<int>{4} },
            new GenreDto { Id = 5, Title = "Genre5", TextCardIds = new List<int>{5} }
        };

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        #region GetAllWithPagingTests

        [Test]
        public async Task GenreController_GetAllWithPaging_ReturnsAllGenres()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.CreateClient();

            var expected = GenreDtos.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<GenreDto>>(stringResponse);

            Assert.That(actual.Data, Is.EqualTo(expected).Using(new GenreDtoComparer()));
        }

        [Test]
        public async Task GenreController_GetAllWithPaging_FromEmptyTable_ReturnsNotFound()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var httpResponse = await _client.GetAsync(RequestUri);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GenreController_GetAllWithPaging_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IGenreService, FakeGenreService>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region GetAllTests

        [Test]
        public async Task GenreController_GetAll_ReturnsAllGenres()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var expected = GenreDtos.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}/all");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<GenreDto>>(stringResponse);

            actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.TextCardIds));
            //Assert.That(actual, Is.EqualTo(expected).Using(new GenreDtoComparer()));
        }

        [Test]
        public async Task GenreController_GetAll_FromEmptyTable_ReturnsNotFound()
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
        public async Task GenreController_GetAll_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IGenreService, FakeGenreService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/all");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region GetByNameTests

        [TestCase("Genre1", 1)]
        [TestCase("Genre2", 2)]
        public async Task GenreController_GetByName_ReturnsGenre(string name, int id)
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.CreateClient();

            var expected = GenreDtos.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{name}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<GenreDto>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected[id - 1]).Using(new GenreDtoComparer()));
        }

        [TestCase("1234")]
        [TestCase("Fict")]
        [TestCase("asd")]
        public async Task GenreController_GetByName_ReturnsNotFound(string name)
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{name}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GenreController_GetByName_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IGenreService, FakeGenreService>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/name");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region AddTests

        [Test]
        public async Task GenreController_AddNew_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var genre6 = new GenreDto { Id = 6, Title = "Genre6", TextCardIds = new List<int>() };
            var expectedLength = GenreDtos.Count() + 1;

            var content = new StringContent(JsonConvert.SerializeObject(genre6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<GenreDto>>(stringResponse);
            var added = actual.Data.FirstOrDefault(g => g.Id == genre6.Id);

            Assert.That(actual.Data.Count(), Is.EqualTo(expectedLength));
            Assert.That(added, Is.EqualTo(genre6).Using(new GenreDtoComparer()));
        }

        [Test]
        public async Task GenreController_AddNew_UnAuthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var genre6 = new GenreDto { Id = 6, Title = "Genre6", TextCardIds = new List<int>() };
            var content = new StringContent(JsonConvert.SerializeObject(genre6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GenreController_AddNew_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IGenreService, FakeGenreService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var genre6 = new GenreDto { Id = 6, Title = "Genre6", TextCardIds = new List<int>() };

            var content = new StringContent(JsonConvert.SerializeObject(genre6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GenreController_AddNew_NullModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IGenreService, FakeGenreService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);
                });
            }).CreateClient();

            GenreDto genre6 = null as GenreDto;

            var content = new StringContent(JsonConvert.SerializeObject(genre6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GenreController_AddNew_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var genreToAdd = new GenreDto { Id = 6, Title = "", TextCardIds = new List<int>() };

            var content = new StringContent(JsonConvert.SerializeObject(genreToAdd), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region UpdateTests

        [Test]
        public async Task GenreController_UpdateExisting_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var genre6 = new GenreDto { Id = 5, Title = "Genre6", TextCardIds = new List<int> { 5 } };

            var content = new StringContent(JsonConvert.SerializeObject(genre6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{genre6.Id}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"{RequestUri}/{genre6.Title}");
            getResponse.EnsureSuccessStatusCode();
            var stringResponse = await getResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<GenreDto>(stringResponse);

            Assert.That(actual, Is.EqualTo(genre6).Using(new GenreDtoComparer()));
        }

        [Test]
        public async Task GenreController_Update_UnAuthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var genre6 = new GenreDto { Id = 5, Title = "Genre6", TextCardIds = new List<int>() };
            var content = new StringContent(JsonConvert.SerializeObject(genre6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{genre6.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GenreController_UpdateExisting_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IGenreService, FakeGenreService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var genre6 = new GenreDto { Id = 5, Title = "Genre6", TextCardIds = new List<int> { 5 } };

            var content = new StringContent(JsonConvert.SerializeObject(genre6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{genre6.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GenreController_Update_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var genreToAdd = new GenreDto { Id = 3, Title = "", TextCardIds = new List<int>() };

            var content = new StringContent(JsonConvert.SerializeObject(genreToAdd), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{genreToAdd.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        #endregion

        #region DeleteTests

        [TestCase(1, 4)]
        public async Task GenreController_DeleteExisting_Success(int id, int newLength)
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
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<GenreDto>>(stringResponse);

            Assert.That(actual.Data.Count(), Is.EqualTo(newLength));
        }

        [Test]
        public async Task GenreController_Delete_UnAuthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var genre6 = new GenreDto { Id = 5, Title = "Genre6", TextCardIds = new List<int>() };
            var httpResponse = await _client.DeleteAsync($"{RequestUri}/{genre6.Id}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GenreController_DeleteExisting_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IGenreService, FakeGenreService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.DeleteAsync($"{RequestUri}/1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion
    }
}
