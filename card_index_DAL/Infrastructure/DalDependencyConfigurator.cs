using card_index_DAL.Data;
using card_index_DAL.Interfaces;
using card_index_DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace card_index_DAL.Infrastructure
{
    /// <summary>
    /// Configures dependency injection in DAL
    /// </summary>
    public static class DalDependencyConfigurator
    {
        /// <summary>
        /// Configures dependency injection in DAL,
        /// takes connection string to create DB context,
        /// if DB is empty - it is filled with initial data,
        /// if not exists - created
        /// </summary>
        /// <param name="serviceCollection">service collection</param>
        /// <param name="connectionString">DB connection string, passed to DAL</param>
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
