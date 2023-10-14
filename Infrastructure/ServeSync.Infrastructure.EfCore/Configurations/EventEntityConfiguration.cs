using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class EventEntityConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
               .IsRequired();

        builder.Property(x => x.Description)
               .IsRequired();

        builder.Property(x => x.Introduction)
               .IsRequired();

        builder.Property(x => x.ImageUrl)
               .IsRequired();

        builder.Property(x => x.Status)
               .IsRequired();

        builder.Property(x => x.StartAt)
               .IsRequired();

        builder.Property(x => x.EndAt)
               .IsRequired();
        
        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(x => x.FullAddress)
                   .IsRequired();

            address.Property(x => x.Longitude)
                   .IsRequired();

            address.Property(x => x.Latitude)
                   .IsRequired();
        });

        builder.Property(x => x.RepresentativeOrganizationId)
               .IsRequired(false);
        
        builder.HasOne(x => x.Activity)
               .WithMany()
               .HasForeignKey(x => x.ActivityId);
        
        builder.HasOne(x => x.RepresentativeOrganization)
               .WithOne()
               .HasForeignKey<Event>(x => x.RepresentativeOrganizationId)
               .IsRequired(false);
    }
}