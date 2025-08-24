using Application.Abstraction;
using Application.Abstraction.Services;
using Application.Dtos.UserDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ICurrentUserService currentUserService;
    public UsersController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        this.unitOfWork = unitOfWork;
        this.currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        IEnumerable<User> users = await unitOfWork.Users.GetAllAsync();
        Dictionary<Guid, User> userMap = users.ToDictionary(u => u.Id); // for efficiency.
        IEnumerable<Role> roles = await unitOfWork.Roles.GetAllAsync();
        IEnumerable<Guid> userIds = users.Select(u => u.Id);
        IEnumerable<UserRole> userRoles = await unitOfWork.UserRoles.FindAsync(ur => userIds.Contains(ur.UserId));

        IEnumerable<UserDto> userDtos = userRoles.GroupBy(ur => ur.UserId)
            .Select(g =>
            {
                var user = userMap[g.Key];
                IEnumerable<Guid> thisUserRoleIds = g.Select(ur => ur.RoleId);
                IEnumerable<Role> thisUserRoles = roles.Where(r => thisUserRoleIds.Contains(r.Id));
                return new UserDto
                {
                    Id = g.Key,
                    Email = user.Email,
                    FullName = user.FullName,
                    Roles = thisUserRoles.ToArray(),
                };
            });
        return userDtos.ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        User? user = await unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        IEnumerable<Role> roles = await unitOfWork.Roles.GetAllAsync();
        IEnumerable<UserRole> userRoles = await unitOfWork.UserRoles.FindAsync(ur => ur.UserId == id);
        IEnumerable<Guid> thisUserRoleIds = userRoles.Select(ur => ur.RoleId);
        IEnumerable<Role> thisUserRoles = roles.Where(r => thisUserRoleIds.Contains(r.Id));
        UserDto userDto = new()
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Roles = thisUserRoles.ToArray(),
        };
        return userDto;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        User? user = await unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // delete every other related item first.
        IEnumerable<UserRole> userRoles = await unitOfWork.UserRoles.FindAsync(ur => ur.UserId == user.Id);
        foreach (var userRole in userRoles)
        {
            userRole.IsDeleted = true;
        }

        IEnumerable<OAuthLogin> oAuthLogins = await unitOfWork.OAuthLogins.FindAsync(oal => oal.UserId == user.Id);
        foreach (var oAuthLogin in oAuthLogins)
        {
            oAuthLogin.IsDeleted = true;
        }

        IEnumerable<RefreshToken> refreshTokens = await unitOfWork.RefreshTokens.FindAsync(rt => rt.UserId == user.Id);
        foreach (var refreshToken in refreshTokens)
        {
            refreshToken.IsDeleted = true;
        }

        IEnumerable<VerificationCode> verificationCodes = await unitOfWork.VerificationCodes.FindAsync(rt => rt.Email == user.Email);
        foreach (var verificationCode in verificationCodes)
        {
            await unitOfWork.VerificationCodes.DeleteAsync(verificationCode.Id);
        }

        // soft delete
        user.IsDeleted = true;

        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }


    // only used to change roles.
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, PutUserDto putUserDto)
    {
        User? user = await unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            return NotFound();
        
        // special case for admin edit their own account: not allow
        // One cannot change that one's account.
        if (currentUserService.UserId == id)
            return Forbid();

        // check if all roles changed in request a valid role id
        IEnumerable<Role> validRoles = await unitOfWork.Roles.GetAllAsync();
        var validRoleIds = validRoles.Select(r => r.Id);
        bool hasInvalidRoleId = putUserDto.RoleIds.Any(r => validRoleIds.Contains(r) == false);
        if (hasInvalidRoleId)
            return BadRequest();

        // Update roles
        IEnumerable<UserRole> existingUserRoles = await unitOfWork.UserRoles.FindAsync(ur => ur.UserId == id);

        // get the old list and new list
        IEnumerable<Guid> existingRoleIds = existingUserRoles.Select(ur => ur.RoleId);
        IEnumerable<Guid> newRoleIds = putUserDto.RoleIds;

        // compare to get the difference
        IEnumerable<Guid> roleIdsToAdd = newRoleIds.Except(existingRoleIds);
        IEnumerable<Guid> roleIdsToRemove = existingRoleIds.Except(newRoleIds);

        // Remove old roles
        foreach (Guid roleIdToRemove in roleIdsToRemove)
        {
            await unitOfWork.UserRoles.DeleteAsync(user.Id, roleIdToRemove);
        }

        // Add new roles
        foreach (var roleIdToAdd in roleIdsToAdd)
        {
            await unitOfWork.UserRoles.AddAsync(new UserRole { UserId = id, RoleId = roleIdToAdd });
        }

        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
