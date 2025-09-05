using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserRoadmapConfiguration : IEntityTypeConfiguration<UserRoadmap>
{
    public void Configure(EntityTypeBuilder<UserRoadmap> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.ToTable(nameof(UserRoadmap));

        builder.HasKey(x => x.Id);
        builder.Property(x =>  x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.UserId).IsRequired();
        builder.HasIndex(x => x.UserId);
        // point to User.Id in UserService

        builder.HasOne<Roadmap>()
            .WithMany()
            .HasForeignKey(x => x.RoadmapId)
            .OnDelete(DeleteBehavior.NoAction);
        // Roadmap table is fixed. no need to config OnDelete behaviour
    }
}
