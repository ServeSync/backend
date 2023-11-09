using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class StudentEventRegisterEntityConfiguration : IEntityTypeConfiguration<StudentEventRegister>
{
    public void Configure(EntityTypeBuilder<StudentEventRegister> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Description)
               .IsRequired(false);

        builder.Property(x => x.RejectReason)
               .IsRequired(false);

        builder.Property(x => x.Status)
               .IsRequired();

        builder.HasOne(x => x.EventRole)
               .WithMany(x => x.StudentEventRegisters)
               .HasForeignKey(x => x.EventRoleId);
        
        builder.HasOne(x => x.Student)
               .WithMany(x => x.EventRegisters)
               .HasForeignKey(x => x.StudentId);
    }
}