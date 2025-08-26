using System.Security.Claims;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Implementations;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            string? UserIdRaw = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(UserIdRaw, out Guid userId)) return userId;
            return null;
        }
    }
}
