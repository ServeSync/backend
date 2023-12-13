using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class InternalProofEntityConfiguration : IEntityTypeConfiguration<InternalProof>
{
    public void Configure(EntityTypeBuilder<InternalProof> builder)
    {
        builder.ToTable("InternalProof");
        
        builder.HasOne(x => x.Proof)
               .WithOne(x => x.InternalProof)
               .HasForeignKey<InternalProof>(x => x.Id);
        
        builder.HasOne(x => x.Student)
               .WithMany()
               .HasForeignKey(x => x.StudentId);

        builder.HasOne(x => x.EventRole)
               .WithMany()
               .HasForeignKey(x => x.EventRoleId);

        builder.HasOne(x => x.Event)
               .WithMany()
               .HasForeignKey(x => x.EventId);
    }
}