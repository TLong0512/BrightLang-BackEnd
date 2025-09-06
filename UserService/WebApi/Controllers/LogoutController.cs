using Application.Abstraction;
using Application.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;

namespace WebApi.Controllers;

[Route("api/Authentication")]
[ApiController]
public class LogoutController : ControllerBase
{
    private readonly ITokenService tokenService;

    public LogoutController(ITokenService tokenService)
    {
        this.tokenService = tokenService;
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        string? refreshTokenIdRaw = Request.Cookies[Constant.RefreshTokenIdName];
        if (refreshTokenIdRaw == null || Guid.TryParse(refreshTokenIdRaw, out Guid refreshTokenId) == false)
        {
            return Forbid();
        }

        CookieHelper.RemoveAuthCookies(Response);

        await tokenService.RevokeRefreshTokenAsync(refreshTokenId);

        return Ok();
    }
}
