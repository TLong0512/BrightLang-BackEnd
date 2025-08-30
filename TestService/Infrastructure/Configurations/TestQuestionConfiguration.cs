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
    public class TestQuestionConfiguration : IEntityTypeConfiguration<TestQuestion>
    {
        public void Configure(EntityTypeBuilder<TestQuestion> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.ToTable(nameof(TestQuestion));

            builder.HasKey(x => new { x.QuestionId, x.TestId });

            builder.HasOne(x => x.Test)
                .WithMany(y => y.TestQuestions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(z => z.TestId)
                .IsRequired();
        }
    }
}
