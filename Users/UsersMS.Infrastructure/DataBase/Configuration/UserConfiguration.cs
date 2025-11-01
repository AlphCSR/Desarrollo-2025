using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Domain.Entities;

namespace UsersMS.Infrastructure.DataBase.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(s => s.Email).IsRequired().HasMaxLength(50);
            builder.Property(s => s.Password).IsRequired();
            builder.Property(s => s.Id).IsRequired();
            builder.Property(s => s.Name).IsRequired();
            builder.Property(s => s.LastName).IsRequired();
            builder.Property(s => s.Phone).IsRequired();
            builder.Property(s => s.Address).IsRequired();
            builder.Property(s => s.Role).IsRequired();
            builder.Property(s => s.State).IsRequired();
        }
    }

}
