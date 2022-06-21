using card_index_DAL.Data;
using card_index_DAL.Interfaces;
using card_index_DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace card_index_DAL.Infrastructure
{
    public static class DalDependencyConfigurator
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<CardIndexDbContext>(options => options.UseSqlServer(connectionString));
            serviceCollection.AddTransient<IAuthorRepository, AuthorRepository>();
            serviceCollection.AddTransient<IGenreRepository, GenreRepository>();
            serviceCollection.AddTransient<IRateDetailRepository, RateDetailRepository>();
            serviceCollection.AddTransient<ITextCardRepository, TextCardRepository>();
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
