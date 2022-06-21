using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace card_index_DAL.Data
{
    public class CardIndexDbContext:IdentityDbContext<User,UserRole,int>
    {
        public CardIndexDbContext(DbContextOptions options) : base(options)
        { }
        //public CardIndexDbContext()
        //{

        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=cardIndexDb;Trusted_Connection=True;");
        //}

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

            builder.Entity<User>()
                .HasMany(u => u.RateDetails)
                .WithOne(rd => rd.User)
                .HasForeignKey(rd => rd.UserId);

            builder.Entity<TextCard>()
                .HasMany(c => c.RateDetails)
                .WithOne(rd => rd.TextCard)
                .HasForeignKey(rd => rd.TextCardId);

            builder.Entity<TextCard>()
                .HasOne(c => c.Genre)
                .WithMany(g => g.TextCards)
                .HasForeignKey(c => c.GenreId);

            builder.Entity<TextCard>()
                .HasMany(c => c.Authors)
                .WithMany(a => a.TextCards);
        }

        public DbSet<TextCard> TextCards { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<RateDetail> RateDetails { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}
