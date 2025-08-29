using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    public class TestAnswerCongfiguration : IEntityTypeConfiguration<TestAnswer>
    {
        public void Configure(EntityTypeBuilder<TestAnswer> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.ToTable(nameof(TestAnswer));

            builder.HasKey(x => new { x.AnswerId, x.TestId });

            builder.HasOne(x => x.Test)
                .WithMany(y => y.TestAnswers)
                .HasForeignKey(z => z.TestId)
                .OnDelete(DeleteBehavior.Cascade);        }
    }
}
