using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.MyAccountDto;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Old password is required.")]
    public string OldPassword { get; set; } = string.Empty;


    [Required(ErrorMessage = "New password is required.")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "New password length must be between 6 and 50 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$",
        ErrorMessage = "New password must contain upper and lower case letter, number and special character.")]
    public string NewPassword { get; set; } = string.Empty;


    [Required(ErrorMessage = "Confirm new password is required.")]
    [Compare(nameof(NewPassword), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
