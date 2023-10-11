using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class OrganizationRepInEventEntityConfiguration : IEntityTypeConfiguration<OrganizationRepInEvent>
{
    public void Configure(EntityTypeBuilder<OrganizationRepInEvent> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Role)
               .IsRequired();

        builder.HasOne(x => x.OrganizationInEvent)
               .WithMany(x => x.Representatives)
               .HasForeignKey(x => x.OrganizationInEventId);

        builder.HasOne(x => x.OrganizationRep)
               .WithMany()
               .HasForeignKey(x => x.OrganizationRepId);
    }
}