using card_index_DAL.Entities;
using System;
using System.Collections.Generic;

namespace CardIndexTests.DalTests.Helpers
{
    public class DalTestsData
    {
        private static Genre genre1 = new Genre { Id = 1, Title = "Genre1" };
        private static Genre genre2 = new Genre { Id = 2, Title = "Genre2" };
        private static Genre genre3 = new Genre { Id = 3, Title = "Genre3" };
        private static Genre genre4 = new Genre { Id = 4, Title = "Genre4" };
        private static Genre genre5 = new Genre { Id = 5, Title = "Genre5" };

        private static Author author1 = new Author { Id = 1, FirstName = "James", LastName = "Benton", YearOfBirth = 1956 };
        private static Author author2 = new Author { Id = 2, FirstName = "Donette", LastName = "Foller", YearOfBirth = 1989 };
        private static Author author3 = new Author { Id = 3, FirstName = "Veronika", LastName = "Donald", YearOfBirth = 1990 };
        private static Author author4 = new Author { Id = 4, FirstName = "Jack", LastName = "Wieser", YearOfBirth = 2000 };
        private static Author author5 = new Author { Id = 5, FirstName = "Arnold", LastName = "Clark", YearOfBirth = 2001 };

        private static Author author1Detail = new Author
        {
            Id = 1,
            FirstName = "James",
            LastName = "Benton",
            YearOfBirth = 1956,
            TextCards = new List<TextCard> { new TextCard() }
        };
        private static Author author2Detail = new Author
        {
            Id = 2,
            FirstName = "Donette",
            LastName = "Foller",
            YearOfBirth = 1989,
            TextCards = new List<TextCard> { new TextCard() }
        };
        private static Author author3Detail = new Author
        {
            Id = 3,
            FirstName = "Veronika",
            LastName = "Donald",
            YearOfBirth = 1990,
            TextCards = new List<TextCard> { new TextCard() }
        };
        private static Author author4Detail = new Author
        {
            Id = 4,
            FirstName = "Jack",
            LastName = "Wieser",
            YearOfBirth = 2000,
            TextCards = new List<TextCard> { new TextCard() }
        };
        private static Author author5Detail = new Author
        {
            Id = 5,
            FirstName = "Arnold",
            LastName = "Clark",
            YearOfBirth = 2001,
            TextCards = new List<TextCard> { new TextCard() }
        };

        private static TextCard card1 = new TextCard
        {
            Id = 1,
            Title = "Card1",
            ReleaseDate = new DateTime(1980, 3, 3),
            CardRating = 0,
            GenreId = 1
        };
        private static TextCard card2 = new TextCard
        {
            Id = 2,
            Title = "Card2",
            ReleaseDate = new DateTime(1980, 3, 3),
            CardRating = 0,
            GenreId = 2
        };
        private static TextCard card3 = new TextCard
        {
            Id = 3,
            Title = "Card3",
            ReleaseDate = new DateTime(1980, 3, 3),
            CardRating = 0,
            GenreId = 3
        };
        private static TextCard card4 = new TextCard
        {
            Id = 4,
            Title = "Card4",
            ReleaseDate = new DateTime(1980, 3, 3),
            CardRating = 0,
            GenreId = 4
        };
        private static TextCard card5 = new TextCard
        {
            Id = 5,
            Title = "Card5",
            ReleaseDate = new DateTime(1980, 3, 3),
            CardRating = 0,
            GenreId = 5
        };

        private static TextCard card1Detail = new TextCard
        {
            Id = 1,
            Title = "Card1",
            ReleaseDate = new DateTime(1980, 3, 3),
            CardRating = 0,
            GenreId = 1,
            Authors = new List<Author> { author1 },
            RateDetails = new List<RateDetail> { rate1, rate2 }
        };
        private static TextCard card2Detail = new TextCard
        {
            Id = 2,
            Title = "Card2",
            ReleaseDate = new DateTime(1980, 3, 3),
            CardRating = 0,
            GenreId = 2,
            Authors = new List<Author> { author2 },
            RateDetails = new List<RateDetail> { rate3 }
        };
        private static TextCard card3Detail = new TextCard
        {
            Id = 3,
            Title = "Card3",
            ReleaseDate = new DateTime(1980, 3, 3),
            CardRating = 0,
            GenreId = 3,
            Authors = new List<Author> { author3 },
            RateDetails = new List<RateDetail>()
        };
        private static TextCard card4Detail = new TextCard
        {
            Id = 4,
            Title = "Card4",
            ReleaseDate = new DateTime(1980, 3, 3),
            CardRating = 0,
            GenreId = 4,
            Authors = new List<Author> { author4 },
            RateDetails = new List<RateDetail>()
        };
        private static TextCard card5Detail = new TextCard
        {
            Id = 5,
            Title = "Card5",
            ReleaseDate = new DateTime(1980, 3, 3),
            CardRating = 0,
            GenreId = 5,
            Authors = new List<Author> { author5 },
            RateDetails = new List<RateDetail>()
        };

        private static RateDetail rate1 = new RateDetail
        {
            Id = 1,
            UserId = 1,
            TextCardId = 1,
            RateValue = 3
        };
        private static RateDetail rate2 = new RateDetail
        {
            Id = 2,
            UserId = 2,
            TextCardId = 1,
            RateValue = 5
        };
        private static RateDetail rate3 = new RateDetail
        {
            Id = 3,
            UserId = 1,
            TextCardId = 2,
            RateValue = 3
        };

        public IEnumerable<RateDetail> ExpectedRateDetails =>
            new[] { rate1, rate2, rate3 };
        public IEnumerable<RateDetail> ExpectedRateDetailsWithDetails =>
            new[] { rate1, rate2, rate3 };
        public IEnumerable<Author> ExpectedAuthors =>
            new[] { author1, author2, author3, author4, author5 };
        public IEnumerable<Author> ExpectedAuthorsWithDetails =>
            new[] { author1Detail, author2Detail, author3Detail, author4Detail, author5Detail };
        public IEnumerable<TextCard> ExpectedTextCards =>
            new[]
                { card1, card2, card3, card4, card5 };
        public IEnumerable<TextCard> ExpectedTextCardsWithDetails =>
            new[] { card1Detail, card2Detail, card3Detail, card4Detail, card5Detail };
        public IEnumerable<Genre> ExpectedGenres =>
            new[] { genre1, genre2, genre3, genre4, genre5 };
        public IEnumerable<Genre> ExpectedGenresWithDetails =>
            new[]
            {
                new Genre { Id=1, Title = "Genre1", TextCards = new List<TextCard>{ new TextCard()} },
                new Genre { Id=2, Title = "Genre2", TextCards = new List<TextCard>{ new TextCard()} },
                new Genre { Id=3, Title = "Genre3", TextCards = new List<TextCard>{ new TextCard()} },
                new Genre { Id=4, Title = "Genre4", TextCards = new List<TextCard>{ new TextCard()} },
                new Genre { Id=5, Title = "Genre5", TextCards = new List<TextCard>{ new TextCard()} }
            };
    }
}
