using System;
using System.Collections.Generic;
using System.Text;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Models;
using card_index_DAL.Entities;

namespace CardIndexTests.BllTests.Helpers
{
    public class DataForEntityTests
    {
        private static readonly DataForEntityTests Data = new DataForEntityTests();

        public IEnumerable<TextCardDto> TextCardDtos => textCardDtos;
        public IEnumerable<AuthorDto> AuthorDtos => authorDtos;
        public IEnumerable<RateDetailDto> RateDetailDtos => rateDetailDtos;
        public IEnumerable<GenreDto> GenreDtos => genreDtos;
        public IEnumerable<UserRoleInfoModel> RoleInfoModels => roleInfoModels;
        public IEnumerable<UserInfoModel> UserInfoModels => userInfoModels;
        public IEnumerable<Author> Authors => authors;
        public IEnumerable<Genre> Genres => genres;
        public IEnumerable<TextCard> TextCards => textCards;
        public IEnumerable<RateDetail> RateDetails => rateDetails;
        public IEnumerable<User> Users => users;
        public IEnumerable<UserRole> Roles => roles;

        private DataForEntityTests()
        { }

        public static DataForEntityTests GetTestData()
        {
            return Data;
        }
        #region Test Data Dto

        private TextCardDto cardDto1 = new TextCardDto
        {
            Id = 1,
            GenreName = "GenreOne",
            Title = "TextCardOne",
            RateDetailsIds = new List<int> { 1, 2 },
            CardRating = 3.0,
            ReleaseDate = new DateTime(1990, 1, 1),
            AuthorIds = new List<int> { 1 }
        };
        private TextCardDto cardDto2 = new TextCardDto
        {
            Id = 2,
            GenreName = "GenreOne",
            Title = "TextCardTwo",
            RateDetailsIds = new List<int> { 3, 4 },
            CardRating = 4.0,
            ReleaseDate = new DateTime(1990, 2, 2),
            AuthorIds = new List<int> { 1 }
        };
        private TextCardDto cardDto3 = new TextCardDto
        {
            Id = 3,
            GenreName = "GenreTwo",
            Title = "TextCardThree",
            RateDetailsIds = new List<int> { 5, 6 },
            CardRating = 3.0,
            ReleaseDate = new DateTime(1991, 1, 1),
            AuthorIds = new List<int> { 2 }
        };
        private TextCardDto cardDto4 = new TextCardDto
        {
            Id = 4,
            GenreName = "GenreTwo",
            Title = "TextCardFour",
            RateDetailsIds = new List<int> { 7, 8 },
            CardRating = 4.0,
            ReleaseDate = new DateTime(1991, 2, 2),
            AuthorIds = new List<int> { 2 }
        };
        private TextCardDto cardDto5 = new TextCardDto
        {
            Id = 5,
            GenreName = "GenreThree",
            Title = "TextCardFive",
            RateDetailsIds = new List<int> { 9, 10 },
            CardRating = 3.0,
            ReleaseDate = new DateTime(1992, 1, 1),
            AuthorIds = new List<int> { 3 }
        };
        private TextCardDto cardDto6 = new TextCardDto
        {
            Id = 6,
            GenreName = "GenreThree",
            Title = "TextCardSix",
            RateDetailsIds = new List<int> { 11, 12 },
            CardRating = 4.0,
            ReleaseDate = new DateTime(1992, 2, 2),
            AuthorIds = new List<int> { 3 }
        };
        private TextCardDto cardDto7 = new TextCardDto
        {
            Id = 7,
            GenreName = "GenreFour",
            Title = "TextCardSeven",
            RateDetailsIds = new List<int> { 13, 14 },
            CardRating = 3.0,
            ReleaseDate = new DateTime(1993, 1, 1),
            AuthorIds = new List<int> { 4 }
        };
        private TextCardDto cardDto8 = new TextCardDto
        {
            Id = 8,
            GenreName = "GenreFour",
            Title = "TextCardEight",
            RateDetailsIds = new List<int> { 15, 16 },
            CardRating = 4.0,
            ReleaseDate = new DateTime(1993, 2, 2),
            AuthorIds = new List<int> { 4 }
        };
        private TextCardDto cardDto9 = new TextCardDto
        {
            Id = 9,
            GenreName = "GenreFive",
            Title = "TextCardNine",
            RateDetailsIds = new List<int> { 17, 18 },
            CardRating = 3.0,
            ReleaseDate = new DateTime(1994, 1, 1),
            AuthorIds = new List<int> { 5 }
        };
        private TextCardDto cardDto10 = new TextCardDto
        {
            Id = 10,
            GenreName = "GenreFive",
            Title = "TextCardTen",
            RateDetailsIds = new List<int> { 19, 20 },
            CardRating = 4.0,
            ReleaseDate = new DateTime(1994, 2, 2),
            AuthorIds = new List<int> { 5 }
        };
        private IEnumerable<TextCardDto> textCardDtos =>
            new List<TextCardDto>
        {
            cardDto1,
            cardDto2,
            cardDto3,
            cardDto4,
            cardDto5,
            cardDto6,
            cardDto7,
            cardDto8,
            cardDto9,
            cardDto10
        };

        private AuthorDto authorDto1 = new AuthorDto
        {
            Id = 1,
            FirstName = "AuthornameOne",
            LastName = "AuthorlastnameOne",
            YearOfBirth = 1981,
            TextCardIds = new List<int> { 1, 2 }
        };
        private AuthorDto authorDto2 = new AuthorDto
        {
            Id = 2,
            FirstName = "AuthornameTwo",
            LastName = "AuthorlastnameTwo",
            YearOfBirth = 1982,
            TextCardIds = new List<int> { 3, 4 }
        };
        private AuthorDto authorDto3 = new AuthorDto
        {
            Id = 3,
            FirstName = "AuthornameThree",
            LastName = "AuthorlastnameThree",
            YearOfBirth = 1983,
            TextCardIds = new List<int> { 5, 6 }
        };
        private AuthorDto authorDto4 = new AuthorDto
        {
            Id = 4,
            FirstName = "AuthornameFour",
            LastName = "AuthorlastnameFour",
            YearOfBirth = 1984,
            TextCardIds = new List<int> { 7, 8 }
        };
        private AuthorDto authorDto5 = new AuthorDto
        {
            Id = 5,
            FirstName = "AuthornameFive",
            LastName = "AuthorlastnameFive",
            YearOfBirth = 1985,
            TextCardIds = new List<int> { 9, 10 }
        };

        private IEnumerable<AuthorDto> authorDtos =>
            new List<AuthorDto>
            {
                authorDto1,
                authorDto2,
                authorDto3,
                authorDto4,
                authorDto5
            };

        private GenreDto genreDto1 = new GenreDto
        {
            Id = 1,
            Title = "GenreOne",
            TextCardIds = new List<int> { 1, 2 }
        };
        private GenreDto genreDto2 = new GenreDto
        {
            Id = 2,
            Title = "GenreTwo",
            TextCardIds = new List<int> { 3, 4 }
        };
        private GenreDto genreDto3 = new GenreDto
        {
            Id = 3,
            Title = "GenreThree",
            TextCardIds = new List<int> { 5, 6 }
        };
        private GenreDto genreDto4 = new GenreDto
        {
            Id = 4,
            Title = "GenreFour",
            TextCardIds = new List<int> { 7, 8 }
        };
        private GenreDto genreDto5 = new GenreDto
        {
            Id = 5,
            Title = "GenreFive",
            TextCardIds = new List<int> { 9, 10 }
        };

        private IEnumerable<GenreDto> genreDtos =>
            new List<GenreDto>
            {
                genreDto1,
                genreDto2,
                genreDto3,
                genreDto4,
                genreDto5
            };

        private RateDetailDto rateDetailDto1 = new RateDetailDto
        {
            Id = 1,
            TextCardId = 1,
            CardName = "TextCardOne",
            FirstName = "",
            LastName = "UserFirst",
            RateValue = 2,
            UserId = 1
        };
        private RateDetailDto rateDetailDto2 = new RateDetailDto
        {
            Id = 2,
            TextCardId = 1,
            CardName = "TextCardOne",
            FirstName = "UserSecond",
            LastName = "UserSecond",
            RateValue = 4,
            UserId = 2
        };
        private RateDetailDto rateDetailDto3 = new RateDetailDto
        {
            Id = 3,
            TextCardId = 2,
            CardName = "TextCardTwo",
            FirstName = "UserFirst",
            LastName = "UserFirst",
            RateValue = 3,
            UserId = 1
        };
        private RateDetailDto rateDetailDto4 = new RateDetailDto
        {
            Id = 4,
            TextCardId = 2,
            CardName = "TextCardTwo",
            FirstName = "UserSecond",
            LastName = "UserSecond",
            RateValue = 5,
            UserId = 2
        };
        private RateDetailDto rateDetailDto5 = new RateDetailDto
        {
            Id = 5,
            TextCardId = 3,
            CardName = "TextCardThree",
            FirstName = "UserFirst",
            LastName = "UserFirst",
            RateValue = 2,
            UserId = 1
        };
        private RateDetailDto rateDetailDto6 = new RateDetailDto
        {
            Id = 6,
            TextCardId = 3,
            CardName = "TextCardThree",
            FirstName = "UserSecond",
            LastName = "UserSecond",
            RateValue = 4,
            UserId = 2
        };
        private RateDetailDto rateDetailDto7 = new RateDetailDto
        {
            Id = 7,
            TextCardId = 4,
            CardName = "TextCardFour",
            FirstName = "UserFirst",
            LastName = "UserFirst",
            RateValue = 3,
            UserId = 1
        };
        private RateDetailDto rateDetailDto8 = new RateDetailDto
        {
            Id = 8,
            TextCardId = 4,
            CardName = "TextCardFour",
            FirstName = "UserSecond",
            LastName = "UserSecond",
            RateValue = 5,
            UserId = 2
        };
        private RateDetailDto rateDetailDto9 = new RateDetailDto
        {
            Id = 9,
            TextCardId = 5,
            CardName = "TextCardFive",
            FirstName = "UserFirst",
            LastName = "UserFirst",
            RateValue = 2,
            UserId = 1
        };
        private RateDetailDto rateDetailDto10 = new RateDetailDto
        {
            Id = 10,
            TextCardId = 5,
            CardName = "TextCardFive",
            FirstName = "UserSecond",
            LastName = "UserSecond",
            RateValue = 4,
            UserId = 2
        };
        private RateDetailDto rateDetailDto11 = new RateDetailDto
        {
            Id = 11,
            TextCardId = 6,
            CardName = "TextCardSix",
            FirstName = "UserThird",
            LastName = "UserThird",
            RateValue = 3,
            UserId = 3
        };
        private RateDetailDto rateDetailDto12 = new RateDetailDto
        {
            Id = 12,
            TextCardId = 6,
            CardName = "TextCardSix",
            FirstName = "UserFourth",
            LastName = "UserFourth",
            RateValue = 5,
            UserId = 4
        };
        private RateDetailDto rateDetailDto13 = new RateDetailDto
        {
            Id = 13,
            TextCardId = 7,
            CardName = "TextCardSeven",
            FirstName = "UserThird",
            LastName = "UserThird",
            RateValue = 2,
            UserId = 3
        };
        private RateDetailDto rateDetailDto14 = new RateDetailDto
        {
            Id = 14,
            TextCardId = 7,
            CardName = "TextCardSeven",
            FirstName = "UserFourth",
            LastName = "UserFourth",
            RateValue = 4,
            UserId = 4
        };
        private RateDetailDto rateDetailDto15 = new RateDetailDto
        {
            Id = 15,
            TextCardId = 8,
            CardName = "TextCardEight",
            FirstName = "UserThird",
            LastName = "UserThird",
            RateValue = 3,
            UserId = 3
        };
        private RateDetailDto rateDetailDto16 = new RateDetailDto
        {
            Id = 16,
            TextCardId = 8,
            CardName = "TextCardEight",
            FirstName = "UserFourth",
            LastName = "UserFourth",
            RateValue = 5,
            UserId = 4
        };
        private RateDetailDto rateDetailDto17 = new RateDetailDto
        {
            Id = 17,
            TextCardId = 9,
            CardName = "TextCardNine",
            FirstName = "UserThird",
            LastName = "UserThird",
            RateValue = 2,
            UserId = 3
        };
        private RateDetailDto rateDetailDto18 = new RateDetailDto
        {
            Id = 18,
            TextCardId = 9,
            CardName = "TextCardNine",
            FirstName = "UserFourth",
            LastName = "UserFourth",
            RateValue = 4,
            UserId = 4
        };
        private RateDetailDto rateDetailDto19 = new RateDetailDto
        {
            Id = 19,
            TextCardId = 10,
            CardName = "TextCardTen",
            FirstName = "UserThird",
            LastName = "UserThird",
            RateValue = 3,
            UserId = 3
        };
        private RateDetailDto rateDetailDto20 = new RateDetailDto
        {
            Id = 20,
            TextCardId = 10,
            CardName = "TextCardTen",
            FirstName = "UserFourth",
            LastName = "UserFourth",
            RateValue = 5,
            UserId = 4
        };

        private IEnumerable<RateDetailDto> rateDetailDtos =>

        new List<RateDetailDto>
        {
            rateDetailDto1,rateDetailDto2,
            rateDetailDto3,rateDetailDto4,
            rateDetailDto5,rateDetailDto6,
            rateDetailDto7,rateDetailDto8,
            rateDetailDto9,rateDetailDto10,
            rateDetailDto11,rateDetailDto12,
            rateDetailDto13,rateDetailDto14,
            rateDetailDto15,rateDetailDto16,
            rateDetailDto17,rateDetailDto18,
            rateDetailDto19,rateDetailDto20
        };

        private UserRoleInfoModel roleInfo1 = new UserRoleInfoModel
        {
            Id = 1,
            RoleName = "Admin"
        };
        private UserRoleInfoModel roleInfo2 = new UserRoleInfoModel
        {
            Id = 2,
            RoleName = "Registered"
        };
        private UserRoleInfoModel roleInfo3 = new UserRoleInfoModel
        {
            Id = 3,
            RoleName = "Moderator"
        };

        private IEnumerable<UserRoleInfoModel> roleInfoModels =>
            new List<UserRoleInfoModel> { roleInfo1, roleInfo2, roleInfo3 };

        private UserInfoModel userInfo1 = new UserInfoModel
        {
            Id = 1,
            FirstName = "UserFirst",
            LastName = "UserFirst",
            City = "CityFirst",
            DateOfBirth = new DateTime(1993, 1, 1),
            Email = "First@mail.com",
            Phone = "+33(888)1234567",
            UserRoles = new List<string> { "Admin" }
        };
        private UserInfoModel userInfo2 = new UserInfoModel
        {
            Id = 2,
            FirstName = "UserSecond",
            LastName = "UserSecond",
            City = "CitySecond",
            DateOfBirth = new DateTime(1993, 2, 2),
            Email = "Second@mail.com",
            Phone = "+33(888)2234567",
            UserRoles = new List<string> { "Registered" }
        };
        private UserInfoModel userInfo3 = new UserInfoModel
        {
            Id = 3,
            FirstName = "UserThird",
            LastName = "UserThird",
            City = "CityThird",
            DateOfBirth = new DateTime(1993, 3, 3),
            Email = "Third@mail.com",
            Phone = "+33(888)3234567",
            UserRoles = new List<string> { "Moderator" }
        };

        private IEnumerable<UserInfoModel> userInfoModels =>
            new List<UserInfoModel> { userInfo1, userInfo2, userInfo3 };
        #endregion

        #region Test Data DAL

        private Author author1 = new Author
        {
            Id = 1,
            FirstName = "AuthornameOne",
            LastName = "AuthorlastnameOne",
            YearOfBirth = 1981
        };
        private Author author2 = new Author
        {
            Id = 2,
            FirstName = "AuthornameTwo",
            LastName = "AuthorlastnameTwo",
            YearOfBirth = 1982
        };
        private Author author3 = new Author
        {
            Id = 3,
            FirstName = "AuthornameThree",
            LastName = "AuthorlastnameThree",
            YearOfBirth = 1983
        };
        private Author author4 = new Author
        {
            Id = 4,
            FirstName = "AuthornameFour",
            LastName = "AuthorlastnameFour",
            YearOfBirth = 1984
        };
        private Author author5 = new Author
        {
            Id = 5,
            FirstName = "AuthornameFive",
            LastName = "AuthorlastnameFive",
            YearOfBirth = 1985
        };

        private IEnumerable<Author> authors =>
            new List<Author> { author1, author2, author3, author4, author5 };

        private Genre genre1 = new Genre
        {
            Id = 1,
            Title = "GenreOne"
        };
        private Genre genre2 = new Genre
        {
            Id = 2,
            Title = "GenreTwo"
        };
        private Genre genre3 = new Genre
        {
            Id = 3,
            Title = "GenreThree"
        };
        private Genre genre4 = new Genre
        {
            Id = 4,
            Title = "GenreFour"
        };
        private Genre genre5 = new Genre
        {
            Id = 5,
            Title = "GenreFive"
        };

        private IEnumerable<Genre> genres =>
            new List<Genre> { genre1, genre2, genre3, genre4, genre5 };

        private TextCard card1 = new TextCard
        {
            Id = 1,
            GenreId = 1,
            Title = "TextCardOne",
            CardRating = 3.0,
            ReleaseDate = new DateTime(1990, 1, 1),
        };
        private TextCard card2 = new TextCard
        {
            Id = 2,
            GenreId = 1,
            Title = "TextCardTwo",
            CardRating = 4.0,
            ReleaseDate = new DateTime(1990, 2, 2)
        };
        private TextCard card3 = new TextCard
        {
            Id = 3,
            GenreId = 2,
            Title = "TextCardThree",
            CardRating = 3.0,
            ReleaseDate = new DateTime(1991, 1, 1)
        };
        private TextCard card4 = new TextCard
        {
            Id = 4,
            GenreId = 2,
            Title = "TextCardFour",
            CardRating = 4.0,
            ReleaseDate = new DateTime(1991, 2, 2)
        };
        private TextCard card5 = new TextCard
        {
            Id = 5,
            GenreId = 3,
            Title = "TextCardFive",
            CardRating = 3.0,
            ReleaseDate = new DateTime(1992, 1, 1)
        };
        private TextCard card6 = new TextCard
        {
            Id = 6,
            GenreId = 3,
            Title = "TextCardSix",
            CardRating = 4.0,
            ReleaseDate = new DateTime(1992, 2, 2),
        };
        private TextCard card7 = new TextCard
        {
            Id = 7,
            GenreId = 4,
            Title = "TextCardSeven",
            CardRating = 3.0,
            ReleaseDate = new DateTime(1993, 1, 1)
        };
        private TextCard card8 = new TextCard
        {
            Id = 8,
            GenreId = 4,
            Title = "TextCardEight",
            CardRating = 4.0,
            ReleaseDate = new DateTime(1993, 2, 2)
        };
        private TextCard card9 = new TextCard
        {
            Id = 9,
            GenreId = 5,
            Title = "TextCardNine",
            CardRating = 3.0,
            ReleaseDate = new DateTime(1994, 1, 1)
        };
        private TextCard card10 = new TextCard
        {
            Id = 10,
            GenreId = 5,
            Title = "TextCardTen",
            CardRating = 4.0,
            ReleaseDate = new DateTime(1994, 2, 2)
        };
        private IEnumerable<TextCard> textCards =>
            new List<TextCard>
        {
            card1,
            card2,
            card3,
            card4,
            card5,
            card6,
            card7,
            card8,
            card9,
            card10
        };

        private RateDetail rateDetail1 = new RateDetail
        {
            Id = 1,
            TextCardId = 1,
            RateValue = 2,
            UserId = 1
        };
        private RateDetail rateDetail2 = new RateDetail
        {
            Id = 2,
            TextCardId = 1,
            RateValue = 4,
            UserId = 2
        };
        private RateDetail rateDetail3 = new RateDetail
        {
            Id = 3,
            TextCardId = 2,
            RateValue = 3,
            UserId = 1
        };
        private RateDetail rateDetail4 = new RateDetail
        {
            Id = 4,
            TextCardId = 2,
            RateValue = 5,
            UserId = 2
        };
        private RateDetail rateDetail5 = new RateDetail
        {
            Id = 5,
            TextCardId = 3,
            RateValue = 2,
            UserId = 1
        };
        private RateDetail rateDetail6 = new RateDetail
        {
            Id = 6,
            TextCardId = 3,
            RateValue = 4,
            UserId = 2
        };
        private RateDetail rateDetail7 = new RateDetail
        {
            Id = 7,
            TextCardId = 4,
            RateValue = 3,
            UserId = 1
        };
        private RateDetail rateDetail8 = new RateDetail
        {
            Id = 8,
            TextCardId = 4,
            RateValue = 5,
            UserId = 2
        };
        private RateDetail rateDetail9 = new RateDetail
        {
            Id = 9,
            TextCardId = 5,
            RateValue = 2,
            UserId = 1
        };
        private RateDetail rateDetail10 = new RateDetail
        {
            Id = 10,
            TextCardId = 5,
            RateValue = 4,
            UserId = 2
        };
        private RateDetail rateDetail11 = new RateDetail
        {
            Id = 11,
            TextCardId = 6,
            RateValue = 3,
            UserId = 3
        };
        private RateDetail rateDetail12 = new RateDetail
        {
            Id = 12,
            TextCardId = 6,
            RateValue = 5,
            UserId = 4
        };
        private RateDetail rateDetail13 = new RateDetail
        {
            Id = 13,
            TextCardId = 7,
            RateValue = 2,
            UserId = 3
        };
        private RateDetail rateDetail14 = new RateDetail
        {
            Id = 14,
            TextCardId = 7,
            RateValue = 4,
            UserId = 4
        };
        private RateDetail rateDetail15 = new RateDetail
        {
            Id = 15,
            TextCardId = 8,
            RateValue = 3,
            UserId = 3
        };
        private RateDetail rateDetail16 = new RateDetail
        {
            Id = 16,
            TextCardId = 8,
            RateValue = 5,
            UserId = 4
        };
        private RateDetail rateDetail17 = new RateDetail
        {
            Id = 17,
            TextCardId = 9,
            RateValue = 2,
            UserId = 3
        };
        private RateDetail rateDetail18 = new RateDetail
        {
            Id = 18,
            TextCardId = 9,
            RateValue = 4,
            UserId = 4
        };
        private RateDetail rateDetail19 = new RateDetail
        {
            Id = 19,
            TextCardId = 10,
            RateValue = 3,
            UserId = 3
        };
        private RateDetail rateDetail20 = new RateDetail
        {
            Id = 20,
            TextCardId = 10,
            RateValue = 5,
            UserId = 4
        };

        private IEnumerable<RateDetail> rateDetails =>

        new List<RateDetail>
        {
            rateDetail1,rateDetail2,
            rateDetail3,rateDetail4,
            rateDetail5,rateDetail6,
            rateDetail7,rateDetail8,
            rateDetail9,rateDetail10,
            rateDetail11,rateDetail12,
            rateDetail13,rateDetail14,
            rateDetail15,rateDetail16,
            rateDetail17,rateDetail18,
            rateDetail19,rateDetail20
        };

        private User user1 = new User
        {
            Id = 1,
            FirstName = "UserFirst",
            LastName = "UserFirst",
            City = "CityFirst",
            DateOfBirth = new DateTime(1993, 1, 1),
            Email = "First@mail.com",
            PhoneNumber = "+33(888)1234567"
        };
        private User user2 = new User
        {
            Id = 2,
            FirstName = "UserSecond",
            LastName = "UserSecond",
            City = "CitySecond",
            DateOfBirth = new DateTime(1993, 2, 2),
            Email = "Second@mail.com",
            PhoneNumber = "+33(888)2234567"
        };
        private User user3 = new User
        {
            Id = 3,
            FirstName = "UserThird",
            LastName = "UserThird",
            City = "CityThird",
            DateOfBirth = new DateTime(1993, 3, 3),
            Email = "Third@mail.com",
            PhoneNumber = "+33(888)3234567"
        };

        private IEnumerable<User> users =>
            new List<User> { user1, user2, user3 };

        private UserRole role1 = new UserRole
        {
            Id = 1,
            Name= "Admin"
        };
        private UserRole role2 = new UserRole
        {
            Id = 2,
            Name = "Registered"
        };
        private UserRole role3 = new UserRole
        {
            Id = 3,
            Name = "Moderator"
        };

        private IEnumerable<UserRole> roles =>
            new List<UserRole> { role1, role2, role3 };

        #endregion
    }
}
