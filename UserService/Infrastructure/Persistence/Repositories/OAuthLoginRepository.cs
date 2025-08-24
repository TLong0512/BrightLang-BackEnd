using Application.Abstraction.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class OAuthLoginRepository : GenericRepository<OAuthLogin, (string, string)>, IOAuthLoginRepository
{
    public OAuthLoginRepository(AppDbContext context) : base(context)
    {
    }
}
