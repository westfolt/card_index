using AutoMapper;
using card_index_BLL.Infrastructure;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CardIndexTests.Helpers
{
    internal static class DbTestHelper
    {
        public static DbContextOptions<CardIndexDbContext> GetTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<CardIndexDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new CardIndexDbContext(options))
            {
                SeedData(context, true);
            }

            return options;
        }

        public static IMapper CreateMapperProfile()
        {
            var myProfile = new CardIndexMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
        public static void SeedData(CardIndexDbContext context, bool initUsers)
        {
            Author author1 = new Author { FirstName = "James", LastName = "Benton", YearOfBirth = 1956 };
            Author author2 = new Author { FirstName = "Donette", LastName = "Foller", YearOfBirth = 1989 };
            Author author3 = new Author { FirstName = "Veronika", LastName = "Donald", YearOfBirth = 1990 };
            Author author4 = new Author { FirstName = "Jack", LastName = "Wieser", YearOfBirth = 2000 };
            Author author5 = new Author { FirstName = "Arnold", LastName = "Clark", YearOfBirth = 2001 };
            
            context.Authors.AddRange(author1, author2, author3, author4, author5);
            context.SaveChanges();

            Genre genre1 = new Genre { Title = "Genre1" };
            Genre genre2 = new Genre { Title = "Genre2" };
            Genre genre3 = new Genre { Title = "Genre3" };
            Genre genre4 = new Genre { Title = "Genre4" };
            Genre genre5 = new Genre { Title = "Genre5" };

            context.Genres.AddRange(genre1, genre2, genre3, genre4, genre5);
            context.SaveChanges();

            TextCard card1 = new TextCard
            {
                Title = "Card1", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreId = 1,
                Authors = new List<Author> { author1 }
            };
            TextCard card2 = new TextCard
            {
                Title = "Card2", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreId = 2,
                Authors = new List<Author> { author2 }
            };
            TextCard card3 = new TextCard
            {
                Title = "Card3", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreId = 3,
                Authors = new List<Author> { author3 }
            };
            TextCard card4 = new TextCard
            {
                Title = "Card4", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreId = 4,
                Authors = new List<Author> { author4 }
            };
            TextCard card5 = new TextCard
            {
                Title = "Card5", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreId = 5,
                Authors = new List<Author> { author5 }
            };

            context.TextCards.AddRange(card1,card2,card3, card4, card5);
            context.SaveChanges();

            RateDetail rate1 = new RateDetail
            {
                UserId = 1,
                TextCardId = 1,
                RateValue = 3
            };
            RateDetail rate2 = new RateDetail
            {
                UserId = 2,
                TextCardId = 1,
                RateValue = 5
            };
            RateDetail rate3 = new RateDetail
            {
                UserId = 1,
                TextCardId = 2,
                RateValue = 3
            };

            context.RateDetails.AddRange(rate1, rate2, rate3);
            context.SaveChanges();
            
            if (initUsers)
            {
                var user1 = new User()
                {
                    FirstName = "Oleksandr",
                    LastName = "Shyman",
                    Email = "mymail@gmail.com",
                    City = "Rivne",
                    UserName = "mymail@gmail.com",
                    DateOfBirth = new DateTime(1988, 12, 12),
                    PhoneNumber = "+38(012)3456789"
                };
                var user2 = new User()
                {
                    FirstName = "Aleksey",
                    LastName = "Grishkov",
                    Email = "newmail@gmail.com",
                    City = "Rivne",
                    UserName = "newmail@gmail.com",
                    DateOfBirth = new DateTime(2001, 01, 12),
                    PhoneNumber = "+38(012)3456789"
                };
                var user3 = new User()
                {
                    FirstName = "Taras",
                    LastName = "Bobkin",
                    Email = "taras@gmail.com",
                    City = "Rivne",
                    UserName = "taras@gmail.com",
                    DateOfBirth = new DateTime(1976, 01, 12),
                    PhoneNumber = "+38(012)3456789"
                };

                context.Users.AddRange(user1, user2, user3);
                context.SaveChanges();
            }
        }

        public static async void SeedData(CardIndexDbContext context, UserManager<User> userManager, RoleManager<UserRole> roleManager)
        {
            SeedData(context, false);
            var roles = new[] { "Admin", "Registered", "Moderator" };
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new UserRole { Name = role });
            }
            var user1 = new User()
            {
                FirstName = "Oleksandr",
                LastName = "Shyman",
                Email = "mymail@gmail.com",
                City = "Rivne",
                UserName = "mymail@gmail.com",
                DateOfBirth = new DateTime(1988, 12, 12),
                PhoneNumber = "+38(012)3456789"
            };
            var user2 = new User()
            {
                FirstName = "Aleksey",
                LastName = "Grishkov",
                Email = "newmail@gmail.com",
                City = "Rivne",
                UserName = "newmail@gmail.com",
                DateOfBirth = new DateTime(2001, 01, 12),
                PhoneNumber = "+38(012)3456789"
            };
            var user3 = new User()
            {
                FirstName = "Taras",
                LastName = "Bobkin",
                Email = "taras@gmail.com",
                City = "Rivne",
                UserName = "taras@gmail.com",
                DateOfBirth = new DateTime(1976, 01, 12),
                PhoneNumber = "+38(012)3456789"
            };

            await userManager.CreateAsync(user1, "_Aq12345678");
            await userManager.CreateAsync(user2, "_Aq12345678");
            await userManager.CreateAsync(user3, "_Aq12345678");
            await userManager.AddToRoleAsync(user1, "Admin");
            await userManager.AddToRoleAsync(user2, "Registered");
            await userManager.AddToRoleAsync(user3, "Moderator");

            RateDetail rate1 = new RateDetail
            {
                UserId = 1,
                TextCardId = 1,
                RateValue = 3
            };
            RateDetail rate2 = new RateDetail
            {
                UserId = 2,
                TextCardId = 1,
                RateValue = 5
            };
            RateDetail rate3 = new RateDetail
            {
                UserId = 1,
                TextCardId = 2,
                RateValue = 3
            };

            await context.RateDetails.AddRangeAsync(rate1, rate2, rate3);
        }
    }
}
