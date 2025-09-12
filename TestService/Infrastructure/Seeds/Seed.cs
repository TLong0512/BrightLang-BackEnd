using Domain.Entities;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeds;

public static class Seed
{
    public static async Task ApplyAsync(
        DefaultContext context,
        IServiceProvider serviceScopeProvider,
        CancellationToken cancellationToken = default)
    {
        // Check if database exists and seeded
        if (context.Database.CanConnect()) return;

        context.Database.Migrate();

        await context.SaveChangesAsync(cancellationToken);
    }
}
