using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class EducationProgramEntityConfiguration : IEntityTypeConfiguration<EducationProgram>
{
    public void Configure(EntityTypeBuilder<EducationProgram> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
               .IsRequired();

        builder.Property(x => x.RequiredCredit)
               .IsRequired();

        builder.Property(x => x.RequiredActivityScore)
               .IsRequired();
    }
}