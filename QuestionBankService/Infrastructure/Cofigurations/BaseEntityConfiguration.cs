using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cofigurations
{
    public class BaseEntityConfiguration
    {
        public static void Configure<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : BaseEntity
        {
            builder.HasQueryFilter(e => e.IsDeleted == false);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.CreatedBy)
                .IsRequired(false);

            builder.Property(e => e.UpdatedBy)
                .IsRequired(false);

            builder.Property(e => e.CreatedDate)
                .IsRequired(false);

            builder.Property(e => e.UpdatedDate)
                .IsRequired(false);

            builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);
        }
    }
}
