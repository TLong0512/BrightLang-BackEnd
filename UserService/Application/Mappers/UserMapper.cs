using Application.Abstraction;
using Application.Dtos.MyAccountDto;
using Domain.Entities;

namespace Application.Mappers;

public static class UserMapper
{
    public static async Task<MyAccountDto> ToMyAccountDtoAsync(
        this User user, IUnitOfWork unitOfWork)
    {
        var userRoles = await unitOfWork.UserRoles.FindAsync(ur => ur.UserId == user.Id);
        var roleIds = userRoles.Select(ur => ur.RoleId);
        var roles = await unitOfWork.Roles.FindAsync(r => roleIds.Contains(r.Id));

        return new MyAccountDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Roles = roles.Select(r => r.RoleName).ToArray()
        };
    }
}
