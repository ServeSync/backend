using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class SpecialProofEntityConfiguration : IEntityTypeConfiguration<SpecialProof>
{
    public void Configure(EntityTypeBuilder<SpecialProof> builder)
    {
        builder.ToTable("SpecialProof");

        builder.Property(x => x.Title)
            .IsRequired();

        builder.Property(x => x.StartAt)
            .IsRequired();
        
        builder.Property(x => x.EndAt)
            .IsRequired();
        
        builder.Property(x => x.Role)
            .IsRequired();
        
        builder.Property(x => x.Score)
            .IsRequired();

        builder.HasOne(x => x.Proof)
            .WithOne(x => x.SpecialProof)
            .HasForeignKey<SpecialProof>(x => x.Id);

        builder.HasOne(x => x.Activity)
            .WithMany()
            .HasForeignKey(x => x.ActivityId);
    }
}