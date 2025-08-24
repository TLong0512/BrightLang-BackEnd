using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.AuthenticationDtos;

public class ResetPasswordDto
{
    [EmailAddress(ErrorMessage = "Not a valid email.")]
    [Required(ErrorMessage = "Email is required.")]
    [MaxLength(100, ErrorMessage = "Email must be less than 100 characters.")]
    public string Email { get; set; } = string.Empty;



    [Required(ErrorMessage = "Verification code is required.")]
    public string VerificationCode { get; set; } = string.Empty;


    [Required(ErrorMessage = "Password is required.")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 50 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$",
        ErrorMessage = "Password must contain upper and lower case letter, number and special character.")]
    public string NewPassword { get; set; } = string.Empty;



    [Required(ErrorMessage = "Confirm Password is required.")]
    [Compare(nameof(NewPassword), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
