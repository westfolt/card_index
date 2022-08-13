using card_index_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace card_index_DAL.Context.Configuration
{
#pragma warning disable CS1591
    public class RateDetailConfiguration : IEntityTypeConfiguration<RateDetail>
    {
        public void Configure(EntityTypeBuilder<RateDetail> builder)
        {
            builder.HasOne(rd => rd.User)
                .WithMany(u => u.RateDetails)
                .HasForeignKey(rd => rd.UserId);

            builder.HasOne(rd => rd.TextCard)
                .WithMany(tc => tc.RateDetails)
                .HasForeignKey(rd => rd.TextCardId);
        }
    }
}
