using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserDtos;

public class PutUserDto
{
    [Required(ErrorMessage = "Roles are required.")]
    public Guid[] RoleIds { get; set; } = [];
}
