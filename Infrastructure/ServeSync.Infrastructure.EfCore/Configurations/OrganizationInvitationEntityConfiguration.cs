using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class OrganizationInvitationEntityConfiguration : IEntityTypeConfiguration<OrganizationInvitation>
{
    public void Configure(EntityTypeBuilder<OrganizationInvitation> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ReferenceId)
               .IsRequired();

        builder.Property(x => x.Type)
               .IsRequired();

        builder.Property(x => x.Code)
               .IsRequired();
    }
}