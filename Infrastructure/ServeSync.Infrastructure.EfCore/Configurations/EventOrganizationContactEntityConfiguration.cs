using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class EventOrganizationContactEntityConfiguration : IEntityTypeConfiguration<EventOrganizationContact>
{
    public void Configure(EntityTypeBuilder<EventOrganizationContact> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IdentityId)
               .IsRequired(false);

        builder.Property(x => x.Name)
               .IsRequired();

        builder.Property(x => x.Gender)
               .IsRequired(false);

        builder.Property(x => x.Birth)
               .IsRequired(false);

        builder.Property(x => x.Email)
               .IsRequired();
        
       builder.Property(x => x.PhoneNumber)
              .IsRequired();

       builder.Property(x => x.ImageUrl)
              .IsRequired();

       builder.Property(x => x.Address)
              .IsRequired(false);

       builder.Property(x => x.Position)
              .IsRequired(false);

       builder.HasOne(x => x.EventOrganization)
              .WithMany(x => x.Contacts)
              .HasForeignKey(x => x.EventOrganizationId);
    }
}