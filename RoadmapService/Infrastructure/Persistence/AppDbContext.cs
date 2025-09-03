using Application.Abstraction.Services;
using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Roadmap> Roadmaps { get; set; }
    public DbSet<RoadmapElement> RoadmapElements { get; set; }
    public DbSet<UserRoadmap> UserRoadmaps { get; set; }
    public DbSet<Process> Processes { get; set; }


    private readonly ICurrentUserService _currentUserService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new RoadmapConfiguration());
        modelBuilder.ApplyConfiguration(new RoadmapElementConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoadmapConfiguration());
        modelBuilder.ApplyConfiguration(new ProcessConfiguration());

        // seed will happen on app startup.
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        PopulateBaseEntity();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        PopulateBaseEntity();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void PopulateBaseEntity()
    {
        Guid? userId = _currentUserService.UserId;
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = userId;
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedBy = userId;
                entry.Entity.ModifiedAt = DateTime.UtcNow;
            }
        }
    }
}
