using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Range = Domain.Entities.Range;


namespace Infrastructure.Cofigurations
{
    public class RangeConfiguration : IEntityTypeConfiguration<Range>
    {
        public void Configure(EntityTypeBuilder<Range> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.ToTable(nameof(Range), b =>
            {
                b.HasCheckConstraint(
                    "CK_QuestionRange_StartLessThanEnd",
                    "[StartQuestionNumber] < [EndQuestionNumber]"
                );
            });

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.StartQuestionNumber)
                .IsRequired();

            builder.Property(x => x.EndQuestionNumber)
                .IsRequired();


            builder.HasOne(x => x.SkillLevel)
                .WithMany(y => y.Ranges)
                .HasForeignKey(x => x.SkillLevelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
