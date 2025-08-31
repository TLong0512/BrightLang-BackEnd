using Application.Abstraction.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class RoadmapRepository : GenericRepository<Roadmap, Guid>, IRoadmapRepository
{
    public RoadmapRepository(AppDbContext context) : base(context)
    {
    }
}
