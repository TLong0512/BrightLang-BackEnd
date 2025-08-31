using Application.Abstraction;
using Application.Abstraction.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence;


public class UnitOfWork : IUnitOfWork, IDisposable
{
    protected readonly AppDbContext context;

    private IProcessRepository? _processes;
    public IProcessRepository Processes => _processes ??= new ProcessRepository(context);



    private IRoadmapElementRepository? _roadmapElements;
    public IRoadmapElementRepository RoadmapElements => _roadmapElements ??= new RoadmapElementRepository(context);


    private IRoadmapRepository? roadmaps;
    public IRoadmapRepository Roadmaps => roadmaps ??= new RoadmapRepository(context);


    private IUserRoadmapRepository? userRoadmaps;
    public IUserRoadmapRepository UserRoadmaps => userRoadmaps ??= new UserRoadmapRepository(context);


    public UnitOfWork(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(
        System.Data.IsolationLevel isolationLevel,
        CancellationToken cancellationToken)
    {
        return await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await context.Database.BeginTransactionAsync(cancellationToken);
    }


    public async Task<int> SaveChangesAsync()
    {
        int result = await context.SaveChangesAsync();
        return result;
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
