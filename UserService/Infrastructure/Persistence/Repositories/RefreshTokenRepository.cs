using Application.Abstraction.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : GenericRepository<RefreshToken, Guid>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext context) : base(context)
    {
    }
}
