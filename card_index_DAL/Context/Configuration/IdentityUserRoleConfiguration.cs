using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace card_index_DAL.Context.Configuration
{
#pragma warning disable CS1591
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
        {
            builder.ToTable("UserRoles");
        }
    }
}
