using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProcessConfiguration : IEntityTypeConfiguration<Process>
{
    public void Configure(EntityTypeBuilder<Process> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.ToTable(nameof(Process));

        builder.HasKey(e => new { e.UserRoadmapId, e.RoadmapElementId });

        builder.HasOne<UserRoadmap>()
            .WithMany()
            .HasForeignKey(e => e.UserRoadmapId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<RoadmapElement>()
            .WithMany()
            .HasForeignKey(e => e.RoadmapElementId)
            .OnDelete(DeleteBehavior.Cascade);
        // Roadmap element table is fixed. no need to config OnDelete behaviour

        builder.Property(e => e.StartDate).IsRequired();

        builder.Property(e => e.EndDate).IsRequired();

        builder.Property(e => e.IsFinished).IsRequired();

        builder.Property(e => e.IsOpened).IsRequired();


    }
}
