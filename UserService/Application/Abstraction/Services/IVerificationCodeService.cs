namespace Application.Abstraction.Services;

public interface IVerificationCodeService
{
    Task<string> GenerateVerificationCode();
}
