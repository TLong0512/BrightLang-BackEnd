using Application.Abstraction.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class RoadmapElementRepository : GenericRepository<RoadmapElement, Guid>, IRoadmapElementRepository
{
    public RoadmapElementRepository(AppDbContext context) : base(context)
    {
    }
}
