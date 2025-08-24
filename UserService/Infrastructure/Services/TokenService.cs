using Application.Abstraction.Services;
using Application.Abstraction;
using Domain.Entities;

namespace Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IJwtService jwtService;
    private readonly IRefreshTokenService refreshTokenService;
    private readonly IUnitOfWork unitOfWork;

    public TokenService(IJwtService jwtService, IRefreshTokenService refreshTokenService, IUnitOfWork unitOfWork)
    {
        this.jwtService = jwtService;
        this.refreshTokenService = refreshTokenService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<(string newAccessToken, string newRefreshToken, Guid newRefreshTokenId)> IssueTokensAsync(User user)
    {
        // define lifetime
        DateTime refreshTokenLifetime = DateTime.UtcNow.AddDays(7);

        // refresh token
        string newRefreshToken = await refreshTokenService.GenerateNewRefreshTokenAsync();
        RefreshToken refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = BCrypt.Net.BCrypt.HashPassword(newRefreshToken),
            ExpireAt = refreshTokenLifetime
        };
        await unitOfWork.RefreshTokens.AddAsync(refreshToken);
        await unitOfWork.SaveChangesAsync();

        // access token
        string newAccessToken = await jwtService.GenerateJwtToken(user);

        return (newAccessToken, newRefreshToken, refreshToken.Id);
    }

    public async Task RevokeRefreshTokenAsync(Guid refreshTokenId)
    {
        var refreshToken = await unitOfWork.RefreshTokens.GetByIdAsync(refreshTokenId);
        if (refreshToken != null && !refreshToken.RevokeAt.HasValue)
        {
            refreshToken.RevokeAt = DateTime.UtcNow;
            await unitOfWork.SaveChangesAsync();
        }
    }

}

