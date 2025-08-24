namespace Application.Abstraction.Services;

public interface IRefreshTokenService
{
    Task<string> GenerateNewRefreshTokenAsync();
}
