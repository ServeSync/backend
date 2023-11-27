using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class EventOrganizationEntityConfiguration : IEntityTypeConfiguration<EventOrganization>
{
    public void Configure(EntityTypeBuilder<EventOrganization> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Email)
            .IsRequired();

        builder.Property(x => x.Description)
            .IsRequired(false);

        builder.Property(x => x.Address)
            .IsRequired(false);

        builder.Property(x => x.ImageUrl)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.IdentityId)
            .IsRequired(false);

        builder.Property(x => x.TenantId)
            .IsRequired(false);
    }
}