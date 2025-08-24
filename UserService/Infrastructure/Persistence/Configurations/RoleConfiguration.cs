using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.ToTable(nameof(Role));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder.Property(r => r.RoleName)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(r => r.RoleName)
            .IsUnique();
    }
}
