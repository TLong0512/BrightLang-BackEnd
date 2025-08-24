using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.AuthenticationDtos;

public class EmailToVerifyDto
{
    [EmailAddress(ErrorMessage = "Not a valid email.")]
    [Required(ErrorMessage = "Email is required.")]
    [MaxLength(100, ErrorMessage = "Email must be less than 100 characters.")]
    public string Email { get; set; } = string.Empty;
}
