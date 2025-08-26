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
    public class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.ToTable(nameof(Level));

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            //level n-1 examtype
            builder.HasOne(x => x.ExamType)
                .WithMany(y => y.Levels)
                .HasForeignKey(x => x.ExamTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
