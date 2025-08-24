using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.AuthenticationDtos;

public class RegisterDto
{
    // bước 1: nhập các trường dưới đây
    [Required(ErrorMessage = "Username is required.")]
    [MaxLength(50, ErrorMessage = "Full name must be less than 50 characters.")]
    public string FullName { get; set; } = string.Empty;



    [EmailAddress(ErrorMessage = "Not a valid email.")]
    [Required(ErrorMessage = "Email is required.")]
    [MaxLength(100, ErrorMessage = "Email must be less than 100 characters.")]
    public string Email { get; set; } = string.Empty;



    [Required(ErrorMessage = "Password is required.")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 50 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$",
        ErrorMessage = "Password must contain upper and lower case letter, number and special character.")]
    public string Password { get; set; } = string.Empty;



    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;


    // sau đó người dùng nhấn enter, browser sẽ gửi yêu cầu xác minh email.
    // user nhập mã được gửi vào email vào trường này

    [Required(ErrorMessage = "Verification code is required.")]
    public string VerificationCode { get; set; } = string.Empty;
}
