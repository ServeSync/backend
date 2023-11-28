using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class ExternalProofEntityConfiguration : IEntityTypeConfiguration<ExternalProof>
{
    public void Configure(EntityTypeBuilder<ExternalProof> builder)
    {
        builder.ToTable("ExternalProof");

        builder.Property(x => x.EventName)
            .IsRequired();

        builder.Property(x => x.StartAt)
            .IsRequired();
        
        builder.Property(x => x.EndAt)
            .IsRequired();
        
        builder.Property(x => x.Address)
            .IsRequired();
        
        builder.Property(x => x.Role)
            .IsRequired();
        
        builder.Property(x => x.Score)
            .IsRequired();

        builder.HasOne(x => x.Proof)
               .WithOne(x => x.ExternalProof)
               .HasForeignKey<ExternalProof>(x => x.Id);

        builder.HasOne(x => x.Activity)
               .WithMany()
               .HasForeignKey(x => x.ActivityId);
    }
}