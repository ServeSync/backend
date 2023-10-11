using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class EventRoleEntityConfiguration : IEntityTypeConfiguration<EventRole>
{
    public void Configure(EntityTypeBuilder<EventRole> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
               .IsRequired();

        builder.Property(x => x.Description)
               .IsRequired();

        builder.Property(x => x.IsNeedApprove)
               .IsRequired();

        builder.Property(x => x.Score)
               .IsRequired();
        
       builder.HasOne(x => x.Event)
              .WithMany(x => x.Roles)
              .HasForeignKey(x => x.EventId)
              .IsRequired();
    }
}