using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class OrganizationInEventEntityConfiguration : IEntityTypeConfiguration<OrganizationInEvent>
{
    public void Configure(EntityTypeBuilder<OrganizationInEvent> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role)
               .IsRequired();

        builder.HasOne(x => x.Event)
               .WithMany(x => x.Organizations)
               .HasForeignKey(x => x.EventId);

        builder.HasOne(x => x.Organization)
               .WithMany(x => x.OrganizationInEvents)
               .HasForeignKey(x => x.OrganizationId);
    }
}