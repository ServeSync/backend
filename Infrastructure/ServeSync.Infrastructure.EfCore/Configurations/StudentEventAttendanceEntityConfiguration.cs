using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class StudentEventAttendanceEntityConfiguration : IEntityTypeConfiguration<StudentEventAttendance>
{
    public void Configure(EntityTypeBuilder<StudentEventAttendance> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.AttendanceAt)
               .IsRequired();

        builder.HasOne(x => x.StudentEventRegister)
               .WithOne(x => x.StudentEventAttendance)
               .HasForeignKey<StudentEventAttendance>(x => x.StudentEventRegisterId);

        builder.HasOne(x => x.EventAttendanceInfo)
               .WithMany()
               .HasForeignKey(x => x.EventAttendanceInfoId);
    }
}