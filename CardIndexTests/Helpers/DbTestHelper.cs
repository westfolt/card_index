using AutoMapper;
using card_index_BLL.Infrastructure;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

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
                SeedData(context);
            }

            return options;
        }

        public static IMapper CreateMapperProfile()
        {
            var myProfile = new CardIndexMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
        public static void SeedData(CardIndexDbContext context)
        {
            context.Authors.AddRange(
                new Author { FirstName = "James", LastName = "Benton", YearOfBirth = 1956 },
                new Author { FirstName = "Donette", LastName = "Foller", YearOfBirth = 1989 },
                new Author { FirstName = "Veronika", LastName = "Donald", YearOfBirth = 1990 },
                new Author { FirstName = "Jack", LastName = "Wieser", YearOfBirth = 2000 },
                new Author { FirstName = "Arnold", LastName = "Clark", YearOfBirth = 2001 });

            context.Genres.AddRange(
                new Genre { Title = "Genre1" },
                new Genre { Title = "Genre2" },
                new Genre { Title = "Genre3" },
                new Genre { Title = "Genre4" },
                new Genre { Title = "Genre5" });

            context.TextCards.AddRange(
                new TextCard { Title = "Card1", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreId = 1 },
                new TextCard { Title = "Card2", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreId = 2 },
                new TextCard { Title = "Card3", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreId = 3 },
                new TextCard { Title = "Card4", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreId = 4 },
                new TextCard { Title = "Card5", ReleaseDate = new DateTime(1980, 3, 3), CardRating = 0, GenreId = 5 });

            context.SaveChanges();
        }

        public static async void SeedData(CardIndexDbContext context, UserManager<User> userManager, RoleManager<UserRole> roleManager)
        {
            SeedData(context);
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
        }
    }
}
