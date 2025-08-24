using Domain.Entities;

namespace Application.Dtos.UserDtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    //public string Password { get; set; } = string.Empty;
    public Role[] Roles { get; set; } = [];
}
