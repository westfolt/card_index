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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using card_index_DAL.Entities;
using card_index_Web_API.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace CardIndexTests.WebApiTests
{
    [TestFixture]
    public class TextCardIntegrationTests
    {
        private CardIndexWebAppFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/card";

        private IEnumerable<TextCardDto> TextCardDtos = new List<TextCardDto>
        {
            new TextCardDto { Id = 1, Title = "Card1", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreName = "Genre1", RateDetailsIds = new List<int>{1,2}, AuthorIds = new List<int>{1} },
            new TextCardDto { Id = 2, Title = "Card2", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreName = "Genre2", RateDetailsIds = new List<int>{3}, AuthorIds = new List<int>{2} },
            new TextCardDto { Id = 3, Title = "Card3", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreName = "Genre3", RateDetailsIds = new List<int>(), AuthorIds = new List<int>{3} },
            new TextCardDto { Id = 4, Title = "Card4", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreName = "Genre4", RateDetailsIds = new List<int>(), AuthorIds = new List<int>{4} },
            new TextCardDto { Id = 5, Title = "Card5", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreName = "Genre5", RateDetailsIds = new List<int>(), AuthorIds = new List<int>{5}}
        };

        private IEnumerable<RateDetailDto> RateDetailDtos = new List<RateDetailDto>
        {
            new RateDetailDto { Id = 1, UserId = 1, TextCardId = 1, RateValue = 3, FirstName = "Oleksandr", LastName = "Shyman", CardName = "Card1"},
            new RateDetailDto {Id = 2, UserId = 2, TextCardId = 1, RateValue = 5, FirstName = "Aleksey", LastName = "Grishkov", CardName = "Card2" },
            new RateDetailDto { Id = 3, UserId = 1, TextCardId = 2, RateValue = 3, FirstName = "Oleksandr", LastName = "Shyman", CardName = "Card2" }
        };

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        #region GetAllByFilterTests

        [Test]
        public async Task TextCardController_GetAllByFilter_ReturnsAllTextCards()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.CreateClient();

            var expected = TextCardDtos.ToList();

            var httpResponse = await _client
                .GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<TextCardDto>>(stringResponse);

            Assert.That(actual.Data, Is.EqualTo(expected).Using(new TextCardDtoComparer()));
        }

        [Test]
        public async Task TextCardController_GetAllByFilter_FromEmptyTable_ReturnsEmptyResponse()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var httpResponse = await _client.GetAsync(RequestUri);
            httpResponse.EnsureSuccessStatusCode();

            var getAllResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");
            getAllResponse.EnsureSuccessStatusCode();
            var stringResponse = await getAllResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<TextCardDto>>(stringResponse);

            Assert.That(actual.TotalNumber, Is.EqualTo(0));
            Assert.That(actual.Data.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task TextCardController_GetAllByFilter_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<ICardService, FakeCardService>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}?PageSize=30&PageNumber=1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region GetByIdTests

        [TestCase(1)]
        [TestCase(2)]
        public async Task TextCardController_GetById_ReturnsTextCard(int id)
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.CreateClient();

            var expected = TextCardDtos.ToList();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{id}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<TextCardDto>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected[id - 1]).Using(new TextCardDtoComparer()));
        }

        [TestCase(1234)]
        [TestCase(-1)]
        [TestCase(0)]
        public async Task TextCardController_GetById_ReturnsNotFound(int id)
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/{id}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task TextCardController_GetById_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<ICardService, FakeCardService>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region AddTests

        [Test]
        public async Task TextCardController_AddNew_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

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
            Assert.That(added, Is.EqualTo(TextCard6).Using(new TextCardDtoComparer()));
        }

        [Test]
        public async Task TextCardController_AddNew_Unauthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var card6 = new TextCardDto
            {
                Id = 6,
                Title = "Card6",
                ReleaseDate = new DateTime(1980, 3, 3),
                CardRating = 0,
                GenreName = "Genre2",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };
            var content = new StringContent(JsonConvert.SerializeObject(card6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task TextCardController_AddNew_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<ICardService, FakeCardService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var card6 = new TextCardDto
            {
                Id = 6,
                Title = "Cardnew6",
                ReleaseDate = new DateTime(1980, 3, 3),
                CardRating = 0,
                GenreName = "Genre",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };

            var content = new StringContent(JsonConvert.SerializeObject(card6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task TextCardController_AddNew_NullModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<ICardService, FakeCardService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);
                });
            }).CreateClient();

            TextCardDto genre6 = null as TextCardDto;

            var content = new StringContent(JsonConvert.SerializeObject(genre6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task TextCardController_AddNew_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();
            var cardToAdd = new TextCardDto
            {
                Id = 6,
                Title = "",
                ReleaseDate = new DateTime(1980, 3, 3),
                CardRating = 0,
                GenreName = "Genre2",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };
            var mockService = new Mock<ICardService>();
            var mockUserService = new Mock<IUserService>();
            var cardController = new CardController(mockService.Object, mockUserService.Object);
            cardController.ModelState.AddModelError("Title", "Title is empty");

            var result = await cardController.Add(cardToAdd);
            var objectResult = (BadRequestObjectResult)result.Result;
            var responseObject = objectResult.Value as Response;

            Assert.That(responseObject?.Succeeded, Is.False);
        }

        #endregion

        #region UpdateTests

        [Test]
        public async Task TextCardController_UpdateExisting_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var card6 = new TextCardDto
            {
                Id = 5,
                Title = "Cardnew6",
                ReleaseDate = new DateTime(1980, 4, 3),
                CardRating = 2,
                GenreName = "Genre1",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };

            var content = new StringContent(JsonConvert.SerializeObject(card6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{card6.Id}", content);
            httpResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"{RequestUri}/{card6.Id}");
            getResponse.EnsureSuccessStatusCode();
            var stringResponse = await getResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<TextCardDto>(stringResponse);

            Assert.That(actual, Is.EqualTo(card6).Using(new TextCardDtoComparer()));
        }

        [Test]
        public async Task TextCardController_Update_Unauthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

            var card6 = new TextCardDto
            {
                Id = 5,
                Title = "Card6",
                ReleaseDate = new DateTime(1980, 3, 3),
                CardRating = 0,
                GenreName = "Genre2",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };
            var content = new StringContent(JsonConvert.SerializeObject(card6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{card6.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task TextCardController_UpdateExisting_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<ICardService, FakeCardService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var card6 = new TextCardDto
            {
                Id = 5,
                Title = "Cardnew6",
                ReleaseDate = new DateTime(1980, 4, 3),
                CardRating = 2,
                GenreName = "Genre1",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };

            var content = new StringContent(JsonConvert.SerializeObject(card6), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{RequestUri}/{card6.Id}", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task TextCardController_Update_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();
            var card6 = new TextCardDto
            {
                Id = 5,
                Title = "Card6",
                ReleaseDate = new DateTime(1980, 3, 3),
                CardRating = 0,
                GenreName = "Genre2",
                RateDetailsIds = new List<int>(),
                AuthorIds = new List<int>()
            };
            var mockService = new Mock<ICardService>();
            var mockUserService = new Mock<IUserService>();
            var cardController = new CardController(mockService.Object, mockUserService.Object);
            cardController.ModelState.AddModelError("Title", "Title is empty");

            var result = await cardController.Update(6, card6);
            var objectResult = (BadRequestObjectResult)result.Result;
            var responseObject = objectResult.Value as Response;

            Assert.That(responseObject?.Succeeded, Is.False);
        }

        #endregion

        #region DeleteTests

        [TestCase(1, 4)]
        public async Task TextCardController_DeleteExisting_Success(int id, int newLength)
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
            var actual = JsonConvert.DeserializeObject<DataShapingResponse<TextCardDto>>(stringResponse);

            Assert.That(actual.Data.Count(), Is.EqualTo(newLength));
        }

        [Test]
        public async Task TextCardController_Delete_Unauthorized()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();

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
            var httpResponse = await _client.DeleteAsync($"{RequestUri}/{TextCard6.Id}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task TextCardController_DeleteExisting_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<ICardService, FakeCardService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.DeleteAsync($"{RequestUri}/1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region GetRatingForUserTests

        [Test]
        public async Task TextCardController_GetRatingForCardUser_ReturnsRating()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var expected = RateDetailDtos.ToList()[0];
            var httpResponse = await _client.GetAsync($"{RequestUri}/rate?cardId=1");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<RateDetailDto>(stringResponse);
            
            Assert.That(actual, Is.EqualTo(expected).Using(new RateDetailDtoComparer()));
        }

        [Test]
        public async Task TextCardController_GetRatingForCardUser_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<ICardService, FakeCardService>();
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var httpResponse = await _client.GetAsync($"{RequestUri}/rate?cardId=1");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region GiveRatingToCardTests

        [Test]
        public async Task TextCardController_GiveRating_Success()
        {
            _factory = new CardIndexWebAppFactory(true);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var rateToAdd = new RateDetailDto()
            {
                Id = 4,
                TextCardId = 3,
                UserId = 1,
                RateValue = 2
            };

            var content = new StringContent(JsonConvert.SerializeObject(rateToAdd), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/rate", content);

            var httpGet = await _client.GetAsync($"{RequestUri}/rate?cardId=3");
            var stringResponse = await httpGet.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<RateDetailDto>(stringResponse);

            actual.Should().BeEquivalentTo(rateToAdd, options => 
                options.Excluding(rd=>rd.Id)
                    .Excluding(rd=>rd.FirstName)
                    .Excluding(rd=>rd.LastName)
                    .Excluding(rd=>rd.CardName));
        }

        [Test]
        public async Task TextCardController_GiveRating_WrongModelError()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.CreateClient();
            var rateToAdd = new RateDetailDto()
            {
                Id = 4,
                TextCardId = 1,
                UserId = 1,
                RateValue = 2
            };
            var mockService = new Mock<ICardService>();
            var mockUserService = new Mock<IUserService>();
            var cardController = new CardController(mockService.Object, mockUserService.Object);
            cardController.ModelState.AddModelError("RateValue", "RateValue not correct");

            var result = await cardController.GiveRatingToCard(rateToAdd);
            var objectResult = (BadRequestObjectResult)result.Result;
            var responseObject = objectResult.Value as Response;

            Assert.That(responseObject?.Succeeded, Is.False);
        }

        [Test]
        public async Task TextCardController_GiveRating_ThrowsException()
        {
            _factory = new CardIndexWebAppFactory(false);
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<ICardService, FakeCardService>();
                    services.AddTransient<IUserService, FakeUserService>();
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            var rateToAdd = new RateDetailDto()
            {
                Id = 4,
                TextCardId = 1,
                UserId = 1,
                RateValue = 2
            };

            var content = new StringContent(JsonConvert.SerializeObject(rateToAdd), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{RequestUri}/rate", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion
    }
}
