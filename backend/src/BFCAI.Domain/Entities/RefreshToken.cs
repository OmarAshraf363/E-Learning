namespace BFCAI.Domain.Entities;

/// <summary>
/// Refresh token issued to a user after login (JWT auth).
/// This is the ONLY table added on top of the legacy MVC schema.
/// </summary>
public class RefreshToken
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    /// <summary>SHA-256 hash of the token; the raw token is never stored.</summary>
    public string TokenHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public bool IsActive => RevokedAt is null && ExpiresAt > DateTime.UtcNow;
}
