using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
               new IdentityRole
                   {
                       Id = "89d2de4c-0218-4774-be8c-27f177f28c9e",
                       Name = "Manager",
                       NormalizedName = "MANAGER"
                   },
                   new IdentityRole
                   {
                       Id = "938f1ab9-ceae-4dff-a4a4-344acfd769f5",
                       Name = "Administrator",
                       NormalizedName = "ADMINISTRATOR"
                   }
               );
        }
    }
}
