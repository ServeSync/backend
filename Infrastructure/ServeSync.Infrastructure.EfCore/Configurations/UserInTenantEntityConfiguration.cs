using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class UserInTenantEntityConfiguration : IEntityTypeConfiguration<UserInTenant>
{
    public void Configure(EntityTypeBuilder<UserInTenant> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.UserId });
        
        builder.Property(x => x.FullName)
               .IsRequired();

        builder.Property(x => x.AvatarUrl)
               .IsRequired();

        builder.HasOne(x => x.User)
               .WithMany(x => x.Tenants)
               .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Tenant)
               .WithMany()
               .HasForeignKey(x => x.TenantId);
    }
}