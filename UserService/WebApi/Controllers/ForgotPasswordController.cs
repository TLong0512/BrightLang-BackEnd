using Application.Abstraction.Services;
using Application.Abstraction;
using Application.Dtos.AuthenticationDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/Authentication")]
[ApiController]
public class ForgotPasswordController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IEmailService emailService;
    private readonly IEmailTemplateService emailTemplateService;
    private readonly IVerificationCodeService verificationCodeService;

    public ForgotPasswordController(IUnitOfWork unitOfWork, IEmailService emailService, IEmailTemplateService emailTemplateService, IVerificationCodeService verificationCodeService)
    {
        this.unitOfWork = unitOfWork;
        this.emailService = emailService;
        this.emailTemplateService = emailTemplateService;
        this.verificationCodeService = verificationCodeService;
    }

    [HttpPost("reset-password-email-request")]
    [AllowAnonymousOnly]
    public async Task<IActionResult> ResetPasswordEmailRequest([FromBody] EmailToVerifyDto emailToVerifyDto)
    {
        // kiểm tra xem có mã hợp lệ khả dụng hiện tại hay không
        IEnumerable<VerificationCode> currentValidVerificationCodes = await unitOfWork.VerificationCodes.FindAsync(vc =>
            vc.Email == emailToVerifyDto.Email &&
            vc.ExpireAt > DateTime.UtcNow);
        if (currentValidVerificationCodes.Any() == true)
        {
            return BadRequest(new
            {
                message =
                "We have sent an email verification code recently. " +
                "Please check your email for the code. " +
                "Or, wait for the old one expired after 5 minutes."
            });
        }

        string verificationCode = await verificationCodeService.GenerateVerificationCode();

        // gửi mail
        EmailTemplate? emailTemplate = await unitOfWork.EmailTemplates.GetByNameAsync("ResetPassword.html");
        string content = emailTemplate?.Content ?? "Mã code đặt lại mật khẩu của bạn là {{VerificationCode}}.";

        string template = await emailTemplateService.RenderTemplateAsync(content,
            new Dictionary<string, string>
            {
                { "{{VerificationCode}}", verificationCode }
            });
        await emailService.SendEmailAsync(emailToVerifyDto.Email, "Đặt lại mật khẩu", template);

        // ghi mã code vào db
        await unitOfWork.VerificationCodes.AddAsync(new VerificationCode
        {
            Email = emailToVerifyDto.Email,
            Code = BCrypt.Net.BCrypt.HashPassword(verificationCode),
            ExpireAt = DateTime.UtcNow.AddMinutes(5)
        });
        await unitOfWork.SaveChangesAsync();

        return Ok(); // "If this email is registered, we’ve sent reset instructions"
    }

    [HttpPost("reset-password")]
    [AllowAnonymousOnly]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        // kiểm tra mã code
        IEnumerable<VerificationCode> verificationCodes = await unitOfWork.VerificationCodes
            .FindAsync(vc => vc.Email == resetPasswordDto.Email);

        if (verificationCodes.Any(vc => vc.ExpireAt > DateTime.UtcNow) == false)
        {
            return BadRequest(); // invalid or expired code
        }

        if (verificationCodes.Any(vc => BCrypt.Net.BCrypt.Verify(resetPasswordDto.VerificationCode, vc.Code) == false))
        {
            return BadRequest(); // invalid or expired code
        }

        // mã code hợp lệ, giờ ta sẽ cập nhật mật khẩu
        User? user = await unitOfWork.Users.GetByEmailAsync(resetPasswordDto.Email);
        if (user == null)
        {
            return NotFound(); // user not found
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
        await unitOfWork.Users.UpdateAsync(user);

        // xóa mã code đã sử dụng
        foreach (VerificationCode verificationCode in verificationCodes)
        {
            await unitOfWork.VerificationCodes.DeleteAsync(verificationCode.Id);
        }

        await unitOfWork.SaveChangesAsync();
        return Ok(new { message = "Please log in." });
    }
}
