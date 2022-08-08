using System;
using System.Collections.Generic;
using System.Text;
using card_index_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace card_index_DAL.Context.Configuration
{
#pragma warning disable CS1591
    public class UserRoleConfiguration:IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("Roles");
        }
    }
}
