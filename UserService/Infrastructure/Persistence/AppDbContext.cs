using Application.Abstraction.Services;
using Domain.Entities;
using Infrastructure.Configurations;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<OAuthLogin> OAuthLogins { get; set; }
    public DbSet<VerificationCode> VerificationCodes { get; set; }
    public DbSet<EmailTemplate> EmailTemplates { get; set; }

    private readonly ICurrentUserService _currentUserService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new OAuthLoginConfiguration());
        modelBuilder.ApplyConfiguration(new VerificationCodeConfiguration());
        modelBuilder.ApplyConfiguration(new EmailTemplateConfiguration());

        // seed (Role) and Admin account will happen on app startup.
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
