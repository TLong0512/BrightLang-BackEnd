using Application.Abstraction.Services;

namespace Infrastructure.Services;

public class RefreshTokenService : IRefreshTokenService
{
    public Task<string> GenerateNewRefreshTokenAsync()
    {
        string newRefreshToken = Guid.NewGuid().ToString("N");
        // "N" just means 32 digits, no hyphens

        return Task.FromResult(newRefreshToken);
    }
}
