using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BaseEntityConfiguration
{
    public static void Configure<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : BaseEntity
    {
        // soft delete
        builder.HasQueryFilter(e => e.IsDeleted == false);

        builder.Property(e => e.CreatedBy)
            .IsRequired(false);

        builder.Property(e => e.ModifiedBy)
            .IsRequired(false);

        builder.Property(e => e.CreatedAt)
            .IsRequired(false);

        builder.Property(e => e.ModifiedAt)
            .IsRequired(false);

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);
    }
}
