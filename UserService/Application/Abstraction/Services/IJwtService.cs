using Domain.Entities;

namespace Application.Abstraction.Services;

public interface IJwtService
{
    Task<string> GenerateJwtToken(User user);
}
