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

            builder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
            });
            builder.Entity<UserRole>(entity =>
            {
                entity.ToTable("Roles");
            });
            builder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.Entity<RateDetail>()
                .HasOne(rd => rd.User)
                .WithMany(u => u.RateDetails)
                .HasForeignKey(rd => rd.UserId);

            builder.Entity<RateDetail>()
                .HasOne(rd => rd.TextCard)
                .WithMany(tc => tc.RateDetails)
                .HasForeignKey(rd => rd.TextCardId);

            builder.Entity<TextCard>()
                .HasOne(c => c.Genre)
                .WithMany(g => g.TextCards)
                .HasForeignKey(c => c.GenreId);

            builder.Entity<TextCard>()
                .HasMany(c => c.Authors)
                .WithMany(a => a.TextCards);
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
