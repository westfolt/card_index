using card_index_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace card_index_DAL.Context.Configuration
{
#pragma warning disable CS1591
    public class TextCardConfiguration : IEntityTypeConfiguration<TextCard>
    {
        public void Configure(EntityTypeBuilder<TextCard> builder)
        {
            builder.HasOne(c => c.Genre)
                .WithMany(g => g.TextCards)
                .HasForeignKey(c => c.GenreId);

            builder.HasMany(c => c.Authors)
                .WithMany(a => a.TextCards);
        }
    }
}
