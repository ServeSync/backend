using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class ApplicationUserInRoleEntityConfiguration : IEntityTypeConfiguration<ApplicationUserInRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserInRole> builder)
    {
        builder.HasKey(x => new { x.UserId, x.RoleId, x.TenantId });

        builder.Property(x => x.TenantId)
               .IsRequired();

        builder.HasOne<ApplicationUser>()
               .WithMany(x => x.Roles)
               .HasForeignKey(x => x.UserId);
        
        builder.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);
        
        builder.HasOne(x => x.Tenant)
               .WithMany()
               .HasForeignKey(x => x.TenantId);
    }
}