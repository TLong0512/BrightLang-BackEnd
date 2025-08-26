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
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.ToTable(nameof(Answer));

            builder.HasOne(x => x.Question)
                .WithMany(y => y.Answers)
                .HasForeignKey(x => x.QuestionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Property(x => x.IsCorrect)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
