using card_index_DAL.Data;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_BLL.Models.Data
{
    public static class DataSeed
    {
        public static async Task Seed(IServiceScope serviceScope)
        {
            var roles = new[] { "Admin", "Registered", "Moderator" };

            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<UserRole>>();
            if (!roleManager.Roles.Any())
            {
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new UserRole { Name = role });
                }
            }

            var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
            var user1 = new User()
            {
                FirstName = "Oleksandr",
                LastName = "Shyman",
                Email = "mymail@gmail.com",
                UserName = "mymail@gmail.com",
                DateOfBirth = new DateTime(1988, 12, 12)
            };
            var user2 = new User()
            {
                FirstName = "Aleksey",
                LastName = "Grishkov",
                Email = "newmail@gmail.com",
                UserName = "newmail@gmail.com",
                DateOfBirth = new DateTime(2001, 01, 12)
            };
            var user3 = new User()
            {
                FirstName = "Taras",
                LastName = "Bobkin",
                Email = "taras@gmail.com",
                UserName = "taras@gmail.com",
                DateOfBirth = new DateTime(1976, 01, 12)
            };

            if (!userManager.Users.Any())
            {
                await userManager.CreateAsync(user1, "_Aq12345678");
                await userManager.CreateAsync(user2, "_Aq12345678");
                await userManager.CreateAsync(user3, "_Aq12345678");
                await userManager.AddToRoleAsync(user1, "Admin");
                await userManager.AddToRoleAsync(user2, "Registered");
                await userManager.AddToRoleAsync(user3, "Moderator");
            }

            var author1 = new Author { FirstName = "James", LastName = "Benton", YearOfBirth = 1956 };
            var author2 = new Author { FirstName = "Donette", LastName = "Foller", YearOfBirth = 1989 };
            var author3 = new Author { FirstName = "Veronika", LastName = "Donald", YearOfBirth = 1990 };
            var author4 = new Author { FirstName = "Jack", LastName = "Wieser", YearOfBirth = 2000 };
            var author5 = new Author { FirstName = "Arnold", LastName = "Clark", YearOfBirth = 2001 };

            var genre1 = new Genre { Title = "Fiction" };
            var genre2 = new Genre { Title = "Adventure" };
            var genre3 = new Genre { Title = "Sport" };
            var genre4 = new Genre { Title = "Science" };
            var genre5 = new Genre { Title = "News" };

            var card1 = new TextCard
            {
                Authors = new List<Author> { author1 },
                Genre = genre1,
                ReleaseDate = DateTime.Today.AddMonths(-2),
                Title = "Myfiction1"
            };
            var card2 = new TextCard
            {
                Authors = new List<Author> { author2 },
                Genre = genre2,
                ReleaseDate = DateTime.Today.AddMonths(-3),
                Title = "Myadventure1"
            };
            var card3 = new TextCard
            {
                Authors = new List<Author> { author3 },
                Genre = genre3,
                ReleaseDate = DateTime.Today.AddMonths(-4),
                Title = "Mysport1"
            };
            var card4 = new TextCard
            {
                Authors = new List<Author> { author4 },
                Genre = genre4,
                ReleaseDate = DateTime.Today.AddMonths(-5),
                Title = "Myscience1"
            };
            var card5 = new TextCard
            {
                Authors = new List<Author> { author5 },
                Genre = genre5,
                ReleaseDate = DateTime.Today.AddMonths(-6),
                Title = "Mynews1"
            };

            var context = serviceScope.ServiceProvider.GetService<CardIndexDbContext>();

            if (!context.Authors.Any())
            {
                context.Authors.AddRange(author1, author2, author3, author4, author5);
                await context.SaveChangesAsync();
            }
            if (!context.TextCards.Any())
            {
                context.TextCards.AddRange(card1, card2, card3, card4, card5);
                await context.SaveChangesAsync();
            }
        }
    }
}
