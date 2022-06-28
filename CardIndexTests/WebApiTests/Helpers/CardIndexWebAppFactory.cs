using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_Web_API;
using CardIndexTests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CardIndexTests.WebApiTests.Helpers
{
    public class CardIndexWebAppFactory : WebApplicationFactory<Startup>
    {
        private readonly bool _seedThis;

        public CardIndexWebAppFactory(bool seedData)
        {
            _seedThis = seedData;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RemoveLibraryDbContextRegistration(services);

                var serviceProvider = GetInMemoryServiceProvider();

                services.AddDbContextPool<CardIndexDbContext>(options =>
                {
                    options.UseInMemoryDatabase(Guid.Empty.ToString());
                    options.UseInternalServiceProvider(serviceProvider);
                });
                if (_seedThis)
                {
                    using (var scope = services.BuildServiceProvider().CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<CardIndexDbContext>();
                        var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                        var roleManager = scope.ServiceProvider.GetService<RoleManager<UserRole>>();
                        DbTestHelper.SeedData(context, userManager, roleManager);
                    }
                }
            });
        }

        private static ServiceProvider GetInMemoryServiceProvider()
        {
            return new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
        }

        private static void RemoveLibraryDbContextRegistration(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<CardIndexDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }
    }
}
