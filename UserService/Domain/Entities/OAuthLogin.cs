namespace Domain.Entities;

public class OAuthLogin : BaseEntity
{
    public string LoginProvider { get; set; } = string.Empty;
    public string ProviderKey { get; set; } = string.Empty;
    public string? ProviderDisplayName { get; set; }
    public Guid UserId { get; set; }
}
