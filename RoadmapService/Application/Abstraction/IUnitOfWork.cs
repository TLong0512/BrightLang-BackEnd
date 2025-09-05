using Application.Abstraction.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Abstraction;

public interface IUnitOfWork
{
    IProcessRepository Processes { get; }
    IRoadmapElementRepository RoadmapElements { get; }
    IRoadmapRepository Roadmaps { get; }
    IUserRoadmapRepository UserRoadmaps { get; }

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(
        System.Data.IsolationLevel isolationLevel,
        CancellationToken cancellationToken);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync();
}
