using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Value)
               .IsUnique();

        builder.HasIndex(x => x.AccessTokenId)
               .IsUnique();

        builder.Property(x => x.AccessTokenId)
               .IsRequired();
        
        builder.Property(x => x.Value)
               .IsRequired();

        builder.Property(x => x.ExpiresAt)
               .IsRequired();
        
        builder.HasOne(x => x.User)
               .WithMany(x => x.RefreshToken)
               .HasForeignKey(x => x.UserId);
    }
}