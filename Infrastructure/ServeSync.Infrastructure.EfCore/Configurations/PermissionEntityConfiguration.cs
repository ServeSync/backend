using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;

namespace ServeSync.Infrastructure.EfCore.Configurations;

public class PermissionEntityConfiguration : IEntityTypeConfiguration<ApplicationPermission>
{
    public void Configure(EntityTypeBuilder<ApplicationPermission> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Name)
               .IsUnique(true);
        
        builder.Property(x => x.Name)
               .IsRequired(true);
        
        builder.HasMany<RolePermission>()
               .WithOne()
               .HasForeignKey(x => x.PermissionId);
    }
}