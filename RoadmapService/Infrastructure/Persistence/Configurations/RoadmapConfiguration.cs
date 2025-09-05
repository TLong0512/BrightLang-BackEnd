using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoadmapConfiguration : IEntityTypeConfiguration<Roadmap>
{
    public void Configure(EntityTypeBuilder<Roadmap> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.ToTable(nameof(Roadmap));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.LevelStartId);
        // FK to Level.Id in QuestionBank Serivce

        builder.HasIndex(e => e.LevelEndId);
        // FK to Level.Id in QuestionBank Serivce
    }
}
