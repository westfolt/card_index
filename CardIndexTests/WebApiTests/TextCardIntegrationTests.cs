using card_index_BLL.Models.Dto;
using CardIndexTests.Helpers;
using CardIndexTests.WebApiTests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Models.DataShaping;

namespace CardIndexTests.WebApiTests
{
    public abstract class TextCardIntegrationTests
    {
        protected CardIndexWebAppFactory _factory;
        protected HttpClient _client;
        protected const string RequestUri = "api/card/";

        protected IEnumerable<TextCardDto> TextCardDtos = new List<TextCardDto>
        {
            new TextCardDto { Id = 1, Title = "Card1", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreName = "Genre1", RateDetailsIds = new List<int>(), AuthorIds = new List<int>() },
            new TextCardDto { Id = 2, Title = "Card2", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreName = "Genre2", RateDetailsIds = new List<int>(), AuthorIds = new List<int>() },
            new TextCardDto { Id = 3, Title = "Card3", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreName = "Genre3", RateDetailsIds = new List<int>(), AuthorIds = new List<int>() },
            new TextCardDto { Id = 4, Title = "Card4", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreName = "Genre4", RateDetailsIds = new List<int>(), AuthorIds = new List<int>() },
            new TextCardDto { Id = 5, Title = "Card5", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreName = "Genre5", RateDetailsIds = new List<int>(), AuthorIds = new List<int>()}
        };
    }

    [TestFixture]
    public class TextCardIntegrationSuccessTests : TextCardIntegrationTests
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
        public async Task TextCardController_GetAll_ReturnsAllTextCards()
        {
            var expected = TextCardDtos.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<TextCardDto>>(stringResponse);

            Assert.That(actual.Data, Is.EqualTo(expected).Using(new TextCardComparer()));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task TextCardController_GetById_ReturnsTextCard(int id)
        {
            var expected = TextCardDtos.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}{id}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<TextCardDto>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected[id - 1]).Using(new TextCardComparer()));
        }
    }

    [TestFixture]
    public class TextCardIntegrationSuccessTestsWithAuthentication : TextCardIntegrationTests
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
        public async Task TextCardController_AddNew_Success()
        {
            var TextCard6 = new TextCardDto
            {
                Id = 6,
                Title = "Cardnew6",
                ReleaseDate = new DateTime(1980, 3, 3),
                CardRating = 0,
                GenreName = "Genre",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };
            var expectedLength = TextCardDtos.Count() + 1;

            var content = new StringContent(JsonConvert.SerializeObject(TextCard6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<TextCardDto>>(stringResponse);
            var added = actual.Data.FirstOrDefault(a => a.Id == TextCard6.Id);

            Assert.That(actual.Data.Count(), Is.EqualTo(expectedLength));
            Assert.That(added, Is.EqualTo(TextCard6).Using(new TextCardComparer()));
        }

        [Test]
        public async Task TextCardController_UpdateExisting_Success()
        {
            var TextCard6 = new TextCardDto
            {
                Id = 5,
                Title = "Cardnew6",
                ReleaseDate = new DateTime(1980, 4, 3),
                CardRating = 2,
                GenreName = "Genre1",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };

            var content = new StringContent(JsonConvert.SerializeObject(TextCard6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}{TextCard6.Id}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"{RequestUri}{TextCard6.Id}");
            getResponse.EnsureSuccessStatusCode();
            var stringResponse = await getResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<TextCardDto>(stringResponse);

            Assert.That(actual, Is.EqualTo(TextCard6).Using(new TextCardComparer()));
        }

        [TestCase(1, 4)]
        public async Task TextCardController_DeleteExisting_Success(int id, int newLength)
        {
            var httpResponse = await _client.DeleteAsync($"{RequestUri}{id}");
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<TextCardDto>>(stringResponse);

            Assert.That(actual.Data.Count(), Is.EqualTo(newLength));
        }
    }

    [TestFixture]
    public class TextCardIntegrationErrorTests : TextCardIntegrationTests
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
        public async Task TextCardController_GetAll_FromEmptyTable_ReturnsNotFound()
        {
            var httpResponse = await _client.GetAsync(RequestUri);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase(1234)]
        [TestCase(-1)]
        [TestCase(0)]
        public async Task TextCardController_GetById_ReturnsTextCard(int id)
        {
            var httpResponse = await _client.GetAsync($"{RequestUri}{id}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task TextCardController_AddNew_Unauthorized()
        {
            var TextCard6 = new TextCardDto
            {
                Id = 6,
                Title = "Card6",
                ReleaseDate = new DateTime(1980, 3, 3),
                CardRating = 0,
                GenreName = "Genre2",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };
            var content = new StringContent(JsonConvert.SerializeObject(TextCard6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task TextCardController_Update_Unauthorized()
        {
            var TextCard6 = new TextCardDto
            {
                Id = 5,
                Title = "Card6",
                ReleaseDate = new DateTime(1980, 3, 3),
                CardRating = 0,
                GenreName = "Genre2",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };
            var content = new StringContent(JsonConvert.SerializeObject(TextCard6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}{TextCard6.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task TextCardController_Delete_Unauthorized()
        {
            var TextCard6 = new TextCardDto
            {
                Id = 5,
                Title = "Card6",
                ReleaseDate = new DateTime(1980, 3, 3),
                CardRating = 0,
                GenreName = "Genre2",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };
            var httpResponse = await _client.DeleteAsync($"{RequestUri}{TextCard6.Id}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
