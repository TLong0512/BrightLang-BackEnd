using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

class RoadmapElementConfiguration : IEntityTypeConfiguration<RoadmapElement>
{
    public void Configure(EntityTypeBuilder<RoadmapElement> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.ToTable(nameof(RoadmapElement));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.RoadmapId).IsRequired();
        builder.HasOne<Roadmap>()
            .WithMany()
            .HasForeignKey(e => e.RoadmapId)
            .OnDelete(DeleteBehavior.NoAction); // FK
        // Roadmap table is fixed. no need to config OnDelete behaviour

        builder.Property(e => e.QuestionPerDay).IsRequired();

        builder.Property(e => e.Order).IsRequired();

        builder.Property(e => e.RangeId)
            .IsRequired();
        builder.HasIndex(e => e.RangeId)
            .IsUnique();
        // FK to Range.Id in QuestionBank service.
    }
}
