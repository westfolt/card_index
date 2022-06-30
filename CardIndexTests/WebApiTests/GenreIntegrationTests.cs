using card_index_BLL.Models.Dto;
using CardIndexTests.Helpers;
using CardIndexTests.WebApiTests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Models.DataShaping;

namespace CardIndexTests.WebApiTests
{
    public abstract class GenreIntegrationTests
    {
        protected CardIndexWebAppFactory _factory;
        protected HttpClient _client;
        protected const string RequestUri = "api/genre/";

        protected IEnumerable<GenreDto> GenreDtos = new List<GenreDto>
        {
            new GenreDto { Id = 1, Title = "Genre1", TextCardIds = new List<int>() },
            new GenreDto { Id = 2, Title = "Genre2", TextCardIds = new List<int>() },
            new GenreDto { Id = 3, Title = "Genre3", TextCardIds = new List<int>() },
            new GenreDto { Id = 4, Title = "Genre4", TextCardIds = new List<int>() },
            new GenreDto { Id = 5, Title = "Genre5", TextCardIds = new List<int>() }
        };
    }

    [TestFixture]
    public class GenreIntegrationSuccessTests : GenreIntegrationTests
    {
        [SetUp]
        public void Initialize()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        [Test]
        public async Task GenreController_GetAll_ReturnsAllGenres()
        {
            var expected = GenreDtos.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<GenreDto>>(stringResponse);

            Assert.That(actual.Data, Is.EqualTo(expected).Using(new GenreComparer()));
        }

        [TestCase("Genre1", 1)]
        [TestCase("Genre2", 2)]
        public async Task GenreController_GetByName_ReturnsGenre(string name, int id)
        {
            var expected = GenreDtos.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}{name}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<GenreDto>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected[id - 1]).Using(new GenreComparer()));
        }
    }

    [TestFixture]
    public class GenreIntegrationSuccessTestsWithAuthentication : GenreIntegrationTests
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
        public async Task GenreController_AddNew_Success()
        {
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
            Assert.That(added, Is.EqualTo(genre6).Using(new GenreComparer()));
        }

        [Test]
        public async Task GenreController_UpdateExisting_Success()
        {
            var genre6 = new GenreDto { Id = 5, Title = "Genre6", TextCardIds = new List<int>() };

            var content = new StringContent(JsonConvert.SerializeObject(genre6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}{genre6.Id}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"{RequestUri}{genre6.Title}");
            getResponse.EnsureSuccessStatusCode();
            var stringResponse = await getResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<GenreDto>(stringResponse);

            Assert.That(actual, Is.EqualTo(genre6).Using(new GenreComparer()));
        }

        [TestCase(1, 4)]
        public async Task GenreController_DeleteExisting_Success(int id, int newLength)
        {
            var httpResponse = await _client.DeleteAsync($"{RequestUri}{id}");
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<GenreDto>>(stringResponse);

            Assert.That(actual.Data.Count(), Is.EqualTo(newLength));
        }
    }

    [TestFixture]
    public class GenreIntegrationErrorTests : GenreIntegrationTests
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
        public async Task GenreController_GetAll_FromEmptyTable_ReturnsNotFound()
        {
            var httpResponse = await _client.GetAsync(RequestUri);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("1234")]
        [TestCase("Fict")]
        [TestCase("asd")]
        public async Task GenreController_GetByName_ReturnsGenre(string name)
        {
            var httpResponse = await _client.GetAsync($"{RequestUri}{name}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GenreController_AddNew_UnAuthorized()
        {
            var genre6 = new GenreDto { Id = 6, Title = "Genre6", TextCardIds = new List<int>() };
            var content = new StringContent(JsonConvert.SerializeObject(genre6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GenreController_Update_UnAuthorized()
        {
            var genre6 = new GenreDto { Id = 5, Title = "Genre6", TextCardIds = new List<int>() };
            var content = new StringContent(JsonConvert.SerializeObject(genre6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}{genre6.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GenreController_Delete_UnAuthorized()
        {
            var genre6 = new GenreDto { Id = 5, Title = "Genre6", TextCardIds = new List<int>() };
            var httpResponse = await _client.DeleteAsync($"{RequestUri}{genre6.Id}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
