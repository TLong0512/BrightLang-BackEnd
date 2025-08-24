using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.MyAccountDto;

public class UpdateAccountDto
{
    //public Guid Id { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [MaxLength(50, ErrorMessage = "Full name must be less than 50 characters.")]
    public string FullName { get; set; } = string.Empty;


    //public string Email { get; set; } = string.Empty;
    //public string Password { get; set; } = string.Empty;
    //public Role[] Roles { get; set; } = [];
}
