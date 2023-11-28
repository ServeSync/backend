using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class ProofEntityConfiguration : IEntityTypeConfiguration<Proof>
{
    public void Configure(EntityTypeBuilder<Proof> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProofType)
               .IsRequired();

        builder.Property(x => x.ProofStatus)
               .IsRequired();

        builder.Property(x => x.Description)
               .IsRequired(false);

        builder.Property(x => x.ImageUrl)
               .IsRequired();

        builder.Property(x => x.AttendanceAt)
               .IsRequired();

        builder.HasOne(x => x.Student)
               .WithMany()
               .HasForeignKey(x => x.StudentId);
    }
}