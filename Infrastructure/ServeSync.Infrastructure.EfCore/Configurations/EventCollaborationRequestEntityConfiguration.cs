using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class EventCollaborationRequestEntityConfiguration : IEntityTypeConfiguration<EventCollaborationRequest>
{
    public void Configure(EntityTypeBuilder<EventCollaborationRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
               .IsRequired();

        builder.Property(x => x.Description)
               .IsRequired();

        builder.Property(x => x.Introduction)
               .IsRequired();

        builder.Property(x => x.Capacity)
               .IsRequired();

        builder.Property(x => x.ImageUrl)
               .IsRequired();

        builder.Property(x => x.StartAt)
               .IsRequired();

        builder.Property(x => x.EndAt)
               .IsRequired();

        builder.Property(x => x.Status)
               .IsRequired();

        builder.Property(x => x.EventId)
               .IsRequired(false);
        
        builder.OwnsOne(x => x.Address, address =>
        {
               address.Property(x => x.FullAddress)
                    .IsRequired();

               address.Property(x => x.Longitude)
                    .IsRequired();

               address.Property(x => x.Latitude)
                    .IsRequired();
        });

        builder.OwnsOne(x => x.Organization, organization =>
        {
               organization.Property(x => x.Name)
                           .IsRequired();

               organization.Property(x => x.Description)
                           .IsRequired(false);

               organization.Property(x => x.Email)
                           .IsRequired();

               organization.Property(x => x.PhoneNumber)
                           .IsRequired();

               organization.Property(x => x.Address)
                           .IsRequired(false);

               organization.Property(x => x.ImageUrl)
                           .IsRequired();
        });

        builder.OwnsOne(x => x.OrganizationContact, contact =>
        {
            contact.Property(x => x.Name)
                   .IsRequired();

            contact.Property(x => x.Email)
                   .IsRequired();

            contact.Property(x => x.PhoneNumber)
                   .IsRequired();

            contact.Property(x => x.Gender)
                   .IsRequired(false);

            contact.Property(x => x.Address)
                   .IsRequired(false);

            contact.Property(x => x.Birth)
                   .IsRequired(false);

            contact.Property(x => x.Position)
                   .IsRequired(false);

            contact.Property(x => x.ImageUrl)
                   .IsRequired();
        });

        builder.HasOne(x => x.Activity)
               .WithMany()
               .HasForeignKey(x => x.ActivityId);
    }
}