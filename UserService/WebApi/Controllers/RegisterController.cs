using Application.Abstraction;
using Application.Abstraction.Services;
using Application.Dtos.AuthenticationDtos;
using Application.Dtos.MyAccountDto;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;
using WebApi.Helpers;
using Application.Mappers;

namespace WebApi.Controllers;

[Route("api/Authentication")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IEmailService emailService;
    private readonly IEmailTemplateService emailTemplateService;
    private readonly ITokenService tokenService;
    private readonly IVerificationCodeService verificationCodeService;

    public RegisterController(IUnitOfWork unitOfWork, IEmailService emailService, IEmailTemplateService emailTemplateService, ITokenService tokenService, IVerificationCodeService verificationCodeService)
    {
        this.unitOfWork = unitOfWork;
        this.emailService = emailService;
        this.emailTemplateService = emailTemplateService;
        this.tokenService = tokenService;
        this.verificationCodeService = verificationCodeService;
    }

    [HttpPost("register")]
    [AllowAnonymousOnly]
    public async Task<ActionResult<MyAccountDto>> Register([FromBody] RegisterDto registerDto)
    {
        if (await unitOfWork.Users.GetByEmailAsync(registerDto.Email) != null)
        {
            ModelState.AddModelError(nameof(RegisterDto.Email), "Email is already used.");
            return Conflict(ModelState);
        }


        IEnumerable<VerificationCode> verificationCodes = await unitOfWork.VerificationCodes
            .FindAsync(vc => vc.Email == registerDto.Email);
        if (verificationCodes.Any() == false)
        {
            ModelState.AddModelError(nameof(RegisterDto.VerificationCode),
                "You did not make any email verification request.");
            return ValidationProblem(ModelState); // 400
        }

        VerificationCode? newestVerificationCode = verificationCodes
            .OrderByDescending(vc => vc.CreatedAt)
            .FirstOrDefault();
        if (newestVerificationCode == null)
        {
            ModelState.AddModelError(nameof(RegisterDto.VerificationCode),
                "Could not found your email verification request.");
            return ValidationProblem(ModelState);
        }

        bool isExpired = newestVerificationCode.ExpireAt < DateTime.UtcNow;
        if (isExpired == true)
        {
            await unitOfWork.VerificationCodes.DeleteAsync(newestVerificationCode.Id);
            await unitOfWork.SaveChangesAsync();

            ModelState.AddModelError(nameof(RegisterDto.VerificationCode),
                "Your code is expired.");
            return ValidationProblem(ModelState); // TODO: maybe 410?
        }

        bool isMatched = BCrypt.Net.BCrypt.Verify(registerDto.VerificationCode, newestVerificationCode.Code);
        if (isMatched == false)
        {
            ModelState.AddModelError(nameof(RegisterDto.VerificationCode), "Verification code is not correct.");
            return ValidationProblem(ModelState);
        }

        // finish validation. now to add new user
        User user = new User
        {
            Email = registerDto.Email,
            FullName = registerDto.FullName,
            Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
        };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        Role? userRole = await unitOfWork.Roles.GetByRoleNameAsync("User"); // cant be null, according to Seed.cs
        await unitOfWork.UserRoles.AddAsync(new UserRole { UserId = user.Id, RoleId = userRole!.Id });

        // clean up
        await unitOfWork.VerificationCodes.DeleteAsync(newestVerificationCode.Id);
        await unitOfWork.SaveChangesAsync();

        // let user login right away.
        (string newAccessToken, string newRefreshToken, Guid newRefreshTokenId) = await tokenService.IssueTokensAsync(user);
        CookieHelper.SetAuthCookies(Response, newAccessToken, newRefreshToken, newRefreshTokenId);


        MyAccountDto myAccountDto = await user.ToMyAccountDtoAsync(unitOfWork);
        return Ok(myAccountDto);
    }





    [HttpPost("register-email-request")]
    [AllowAnonymousOnly]
    public async Task<IActionResult> VerifyEmailRequest([FromBody] EmailToVerifyDto emailToVerifyDto)
    {
        if (await unitOfWork.Users.GetByEmailAsync(emailToVerifyDto.Email) != null)
        {
            ModelState.AddModelError(nameof(RegisterDto.Email), "Email is already used.");
            return Conflict(ModelState); // 409
        }

        // kiểm tra xem có mã hợp lệ khả dụng hiện tại hay không
        IEnumerable<VerificationCode> currentValidVerificationCodes = await unitOfWork.VerificationCodes.FindAsync(vc =>
            vc.Email == emailToVerifyDto.Email &&
            vc.ExpireAt > DateTime.UtcNow);
        if (currentValidVerificationCodes.Any() == true)
        {
            return BadRequest(); // 400, or forbidden 403
        }

        string verificationCode = await verificationCodeService.GenerateVerificationCode();

        // gửi mail
        EmailTemplate? emailTemplate = await unitOfWork.EmailTemplates.GetByNameAsync("VerifyEmail.html");
        string content = emailTemplate?.Content ?? "Mã của bạn là {{VerificationCode}}.";

        string template = await emailTemplateService.RenderTemplateAsync(content,
            new Dictionary<string, string>
            {
                { "{{VerificationCode}}", verificationCode }
            });
        await emailService.SendEmailAsync(emailToVerifyDto.Email, "Verify your email", template);

        // ghi mã code vào db
        await unitOfWork.VerificationCodes.AddAsync(new VerificationCode
        {
            Email = emailToVerifyDto.Email,
            Code = BCrypt.Net.BCrypt.HashPassword(verificationCode),
            ExpireAt = DateTime.UtcNow.AddMinutes(5)
        });
        await unitOfWork.SaveChangesAsync();

        return Ok();
    }
}
