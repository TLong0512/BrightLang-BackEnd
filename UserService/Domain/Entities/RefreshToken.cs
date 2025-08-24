namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpireAt { get; set; }
    public DateTime? RevokeAt { get; set; }
    //public byte[] RowVersion { get; set; } = []; // used for optimistic concurrency



    // helper method
    //public bool IsExpired => DateTime.UtcNow >= ExpireAt; 
    //public bool IsActive => RevokeAt == null && !IsExpired;
}