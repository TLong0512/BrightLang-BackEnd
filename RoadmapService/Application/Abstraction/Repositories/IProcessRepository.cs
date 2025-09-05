using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Abstraction.Repositories;

public interface IProcessRepository
{
    Task<Process?> GetByIdAsync(Guid userRoadmapId, Guid roadmapElementId);
    Task<IEnumerable<Process>> GetAllAsync();
    Task<IEnumerable<Process>> FindAsync(Expression<Func<Process, bool>> predicate);


    Task AddAsync(Process entity);
    Task UpdateAsync(Process entity);
    Task DeleteAsync(Guid userRoadmapId, Guid roadmapElementId);

}
