using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Domain.Entities;

namespace NotificationsMS.Infrastructure.DataBase.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(s => s.IdNotification).IsRequired();
            builder.Property(s => s.IdUser).IsRequired();
            builder.Property(s => s.Message).IsRequired();
            builder.Property(s => s.State).IsRequired().HasConversion<string>();
            builder.Property(s => s.CreatedAt).IsRequired();
        }
    }
}
