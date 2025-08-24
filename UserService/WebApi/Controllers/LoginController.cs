using Application.Abstraction;
using Application.Abstraction.Services;
using Application.Dtos.AuthenticationDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Attributes;
using Application.Mappers;
using WebApi.Helpers;
using Application.Dtos.MyAccountDto;

namespace WebApi.Controllers;

[Route("api/Authentication")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ITokenService tokenService;

    public LoginController(IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        this.unitOfWork = unitOfWork;
        this.tokenService = tokenService;
    }

    [HttpPost("login")]
    [AllowAnonymousOnly]
    public async Task<ActionResult<MyAccountDto>> Login([FromBody] LoginDto loginDto)
    {
        User? user = await unitOfWork.Users.GetByEmailAsync(loginDto.Email);
        if (user == null || BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password) == false)
        {
            return Unauthorized(new { message = "Please check your email and password." });
        }

        // xử lý
        (string newAccessToken, string newRefreshToken, Guid newRefreshTokenId) = await tokenService.IssueTokensAsync(user);
        CookieHelper.SetAuthCookies(Response, newAccessToken, newRefreshToken, newRefreshTokenId);

        MyAccountDto myAccountDto = await user.ToMyAccountDtoAsync(unitOfWork);
        return Ok(myAccountDto);
    }



    // 1. Send normal business request with access token.
    // 2. If response is 200 → done.
    // 3. If response is 401 (access token expired/invalid):
    // 3.1. The client doesn’t need to check if a refresh token exists; it simply calls /refresh.
    // 3.2. The browser automatically sends the HttpOnly refresh token cookie along with the request.
    // 4. Server responds to /refresh:
    // 4.1. 2xx → refresh succeeded
    // 4.1.1. Server returns a new access token(and optionally a new refresh token).
    // 4.1.2. Client retries the original request with the new access token.
    // 4.2. Error → refresh failed
    // 4.2.1. Browser/client shows login screen because both tokens are effectively invalid/expired.
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        // validate refresh token
        // refresh token not found => reject
        // client must send the refresh token (ofc automatically because its cookie)
        string? refreshTokenFromRequest = Request.Cookies[Constant.RefreshTokenName];
        if (string.IsNullOrEmpty(refreshTokenFromRequest))
            return BadRequest();

        // the refresh token id also.
        string? refreshTokenFromRequestIdRaw = Request.Cookies[Constant.RefreshTokenIdName];
        if (string.IsNullOrEmpty(refreshTokenFromRequestIdRaw))
            return BadRequest(); // if this one hits this is mostly the server fault.

        if (Guid.TryParse(refreshTokenFromRequestIdRaw, out Guid currentRefreshTokenId) == false)
            return BadRequest(); // if this one hits this is mostly the server fault.

        // find the records in db
        RefreshToken? currentRefreshToken = await unitOfWork.RefreshTokens.GetByIdAsync(currentRefreshTokenId);

        // found none
        if (currentRefreshToken == null)
            return Unauthorized();

        // if the token is being revoked
        if (currentRefreshToken.RevokeAt.HasValue)
            return Unauthorized();

        // if it is expired
        if (currentRefreshToken.ExpireAt <= DateTime.UtcNow)
            return Unauthorized();

        // finally, check if the token is matched.
        if (BCrypt.Net.BCrypt.Verify(refreshTokenFromRequest, currentRefreshToken.Token) == false)
            return Unauthorized();

        // TODO: add a background job / cleanup service to remove expired refresh tokens from DB.

        // main job starts from now
        // get user for next step
        User? user = await unitOfWork.Users.GetByIdAsync(currentRefreshToken.UserId);

        // for some reason User is null (idk maybe it is deleted?)
        if (user == null)
            return Unauthorized();

        // rotate refresh token 
        // Invalidate old refresh token

        // handle race condition
        // TODO: maybe implement the Optimistic Concurrency (add Timestamp column)
        using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            currentRefreshToken.RevokeAt = DateTime.UtcNow;
            await unitOfWork.SaveChangesAsync();

            // generate new tokens and save them
            (string newAccessToken, string newRefreshToken, Guid newRefreshTokenId) = await tokenService.IssueTokensAsync(user);
            CookieHelper.SetAuthCookies(Response, newAccessToken, newRefreshToken, newRefreshTokenId);

            await transaction.CommitAsync();
            return Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync();
            return Conflict();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
