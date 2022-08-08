using card_index_DAL.Context.Configuration;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace card_index_DAL.Data
{
    /// <summary>
    /// Application database context, inherits from IdentityDbContext,
    /// sets it's identifier type to integer
    /// </summary>
    public class CardIndexDbContext : IdentityDbContext<User, UserRole, int>
    {
        /// <summary>
        /// Constructor, takes database context option as parameter
        /// </summary>
        /// <param name="options">Db context options</param>
        public CardIndexDbContext(DbContextOptions<CardIndexDbContext> options) : base(options)
        { }
        /// <summary>
        /// Configures DB schema
        /// </summary>
        /// <param name="builder">Model builder, API to work with DB</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //applying configs to set proper names for identity tables
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
            builder.ApplyConfiguration(new IdentityRoleClaimConfiguration());
            builder.ApplyConfiguration(new IdentityUserClaimConfiguration());
            builder.ApplyConfiguration(new IdentityUserLoginConfiguration());
            builder.ApplyConfiguration(new IdentityUserTokenConfiguration());

            //applying app tables configs
            builder.ApplyConfiguration(new RateDetailConfiguration());
            builder.ApplyConfiguration(new TextCardConfiguration());
        }
        /// <summary>
        /// Set of text cards, stored in DB
        /// </summary>
        public DbSet<TextCard> TextCards { get; set; }
        /// <summary>
        /// Set of authors, stored in DB
        /// </summary>
        public DbSet<Author> Authors { get; set; }
        /// <summary>
        /// Set of rate details, stored in DB
        /// </summary>
        public DbSet<RateDetail> RateDetails { get; set; }
        /// <summary>
        /// Set of genres, stored in DB
        /// </summary>
        public DbSet<Genre> Genres { get; set; }
    }
}
