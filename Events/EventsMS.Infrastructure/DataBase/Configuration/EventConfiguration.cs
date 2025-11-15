using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EventsMS.Domain.Entities;

namespace EventsMS.Infrastructure.DataBase.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Description).HasMaxLength(2000);
            builder.Property(e => e.Location).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Category).IsRequired().HasMaxLength(100);
            builder.Property(e => e.OrganizerId).IsRequired();
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.Capacity).IsRequired();
        }
    }
}