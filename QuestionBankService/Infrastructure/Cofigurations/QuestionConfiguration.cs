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
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.ToTable(nameof(Question));

            builder.HasOne(x => x.Context)
                .WithMany(y => y.Questions)
                .HasForeignKey(x => x.ContextId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Content)
                .IsRequired();
        }
    }
}
