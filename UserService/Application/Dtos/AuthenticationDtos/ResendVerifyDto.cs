using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.AuthenticationDtos;

public class ResendVerifyDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    [StringLength(6, ErrorMessage = "The code must be 6 characters long.")]
    public string Code { get; set; } = string.Empty;
}
