using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class EventActivityEntityConfiguration : IEntityTypeConfiguration<EventActivity>
{
    public void Configure(EntityTypeBuilder<EventActivity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
               .IsRequired();

        builder.Property(x => x.MinScore)
               .IsRequired();

        builder.Property(x => x.MaxScore)
               .IsRequired();

        builder.HasOne(x => x.EventCategory)
               .WithMany(x => x.Activities)
               .HasForeignKey(x => x.EventCategoryId);
    }
}