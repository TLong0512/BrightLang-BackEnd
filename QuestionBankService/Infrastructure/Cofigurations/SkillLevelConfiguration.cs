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
    public class SkillLevelConfiguration : IEntityTypeConfiguration<SkillLevel>
    {
        public void Configure(EntityTypeBuilder<SkillLevel> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.ToTable(nameof(SkillLevel));

            builder.HasOne(x => x.Skill)
                .WithMany(y => y.SkillLevels)
                .HasForeignKey(x => x.SkillId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Level)
                .WithMany(y => y.SkillLevels)
                .HasForeignKey(x => x.LevelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
