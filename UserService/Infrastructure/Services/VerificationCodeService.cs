using Application.Abstraction.Services;
using System.Security.Cryptography;

namespace Infrastructure.Services;

public class VerificationCodeService : IVerificationCodeService
{
    public Task<string> GenerateVerificationCode()
    {
        // 6 chữ số ngẫu nhiên
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        int value = BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF;
        string verificationCode = (value % 1000000).ToString("D6"); // AI generated.

        return Task.FromResult(verificationCode);
    }
}
