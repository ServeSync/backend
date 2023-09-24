using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class HomeRoomEntityConfiguration : IEntityTypeConfiguration<HomeRoom>
{
    public void Configure(EntityTypeBuilder<HomeRoom> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
               .IsRequired();

        builder.Property(x => x.FacultyId)
               .IsRequired();

        builder.HasIndex(x => x.Name)
               .IsUnique();
        
        builder.HasOne(x => x.Faculty)
               .WithMany()
               .HasForeignKey(x => x.FacultyId);
    }
}