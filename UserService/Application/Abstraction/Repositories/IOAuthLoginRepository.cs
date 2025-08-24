using Domain.Entities;

namespace Application.Abstraction.Repositories;

public interface IOAuthLoginRepository : IGenericRepository<OAuthLogin, (string, string)>
{
}
