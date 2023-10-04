using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class FacultyEntityConfiguration : IEntityTypeConfiguration<Faculty>
{
    public void Configure(EntityTypeBuilder<Faculty> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
               .IsRequired();

        builder.HasIndex(x => x.Name)
               .IsUnique();
    }
}