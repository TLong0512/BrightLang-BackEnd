namespace Domain.Entities;

public class VerificationCode : BaseEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime ExpireAt { get; set; }
}
