using Application.Abstraction;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Mappers;
using Application.Abstraction.Services;
using Application.Dtos.MyAccountDto;

namespace WebApi.Controllers;

[Route("api/my-account")]
[ApiController]
[Authorize]
public class MyAccountController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ICurrentUserService currentUserService;

    public MyAccountController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        this.unitOfWork = unitOfWork;
        this.currentUserService = currentUserService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<MyAccountDto>> GetCurrentUser()
    {
        // get the current user id from the token
        string? userIdRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdRaw == null || Guid.TryParse(userIdRaw, out Guid userId) == false)
            return Unauthorized();

        // get the user from db
        User? user = await unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return NotFound();

        MyAccountDto myAccountDto = await user.ToMyAccountDtoAsync(unitOfWork);
        return Ok(myAccountDto);
    }


    [HttpPut("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        Guid? asdf = currentUserService.UserId;
        if (asdf == null) return Unauthorized(); // if this hit its server fault

        User? user = await unitOfWork.Users.GetByIdAsync(asdf.Value);
        if (user == null) return NotFound();

        if (BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, user.Password) == false)
        {
            ModelState.AddModelError(nameof(ChangePasswordDto.OldPassword), "Wrong password.");
            return ValidationProblem(ModelState);
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
        await unitOfWork.SaveChangesAsync();
        return Ok();
    }


    [HttpPut("update-account")]
    public async Task<ActionResult> UpdateAccount([FromBody] UpdateAccountDto updateAccountDto)
    {
        Guid? asdf = currentUserService.UserId;
        if (asdf == null) return Unauthorized(); // if this hit its server fault

        User? user = await unitOfWork.Users.GetByIdAsync(asdf.Value);
        if (user == null) return NotFound();

        // update fields
        user.FullName = updateAccountDto.FullName;
        await unitOfWork.SaveChangesAsync();
        return Ok();
    }

    // TODO: never will i implement delete account. im too tired. maybe one day.
}
