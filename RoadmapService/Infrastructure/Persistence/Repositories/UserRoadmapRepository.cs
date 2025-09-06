using Application.Abstraction.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class UserRoadmapRepository : GenericRepository<UserRoadmap, Guid>, IUserRoadmapRepository
{
    public UserRoadmapRepository(AppDbContext context) : base(context)
    {
    }
}
