using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Range = Domain.Entities.Range;

namespace Infrastructure.Cofigurations
{
    public class ContextConfiguration : IEntityTypeConfiguration<Context>
    {
        public void Configure(EntityTypeBuilder<Context> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.ToTable(nameof(Context));

            builder.HasOne(x => x.Range)
                .WithMany(y => y.Contexts)
                .HasForeignKey(x => x.RangeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.IsBelongTest)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
