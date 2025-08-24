using Domain.Entities;

namespace Application.Abstraction.Services;

public interface ITokenService
{
    Task<(string newAccessToken, string newRefreshToken, Guid newRefreshTokenId)> IssueTokensAsync(User user);
    Task RevokeRefreshTokenAsync(Guid refreshTokenId);
}

