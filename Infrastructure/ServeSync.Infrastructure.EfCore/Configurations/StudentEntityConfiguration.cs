using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Code)
               .IsUnique();

        builder.HasIndex(x => x.CitizenId)
               .IsUnique();
        
        builder.Property(x => x.Code)
               .IsRequired();

        builder.Property(x => x.FullName)
               .IsRequired();

        builder.Property(x => x.Gender)
               .IsRequired();

        builder.Property(x => x.DateOfBirth)
               .IsRequired();

        builder.Property(x => x.HomeTown)
               .IsRequired(false);
        
       builder.Property(x => x.Address)
               .IsRequired(false);

       builder.Property(x => x.ImageUrl)
              .IsRequired();

       builder.Property(x => x.CitizenId)
              .IsRequired();

       builder.Property(x => x.Email)
              .IsRequired();

       builder.Property(x => x.Phone)
              .IsRequired();

       builder.Property(x => x.HomeRoomId)
              .IsRequired();

       builder.Property(x => x.EducationProgramId)
              .IsRequired();

       builder.Property(x => x.IdentityId)
              .IsRequired();

       builder.HasOne(x => x.HomeRoom)
              .WithMany()
              .HasForeignKey(x => x.HomeRoomId);

       builder.HasOne(x => x.EducationProgram)
              .WithMany()
              .HasForeignKey(x => x.EducationProgramId);
    }
}