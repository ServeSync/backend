using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class EventRegistrationInfoEntityConfiguration : IEntityTypeConfiguration<EventRegistrationInfo>
{
    public void Configure(EntityTypeBuilder<EventRegistrationInfo> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.StartAt)
               .IsRequired();

        builder.Property(x => x.EndAt)
               .IsRequired();

        builder.HasOne(x => x.Event)
               .WithMany(x => x.RegistrationInfos)
               .HasForeignKey(x => x.EventId);
    }
}