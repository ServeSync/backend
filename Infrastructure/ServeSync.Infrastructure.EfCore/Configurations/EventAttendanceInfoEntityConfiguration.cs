using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class EventAttendanceInfoEntityConfiguration : IEntityTypeConfiguration<EventAttendanceInfo>
{
    public void Configure(EntityTypeBuilder<EventAttendanceInfo> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
               .IsRequired();

        builder.Property(x => x.StartAt)
               .IsRequired();
        
        builder.Property(x => x.EndAt)
               .IsRequired();

        builder.Property(x => x.QrCodeUrl)
               .IsRequired(false);

        builder.HasOne(x => x.Event)
               .WithMany(x => x.AttendanceInfos)
               .HasForeignKey(x => x.EventId);
    }
}