namespace BFCAI.Infrastructure.Identity;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "BFCAI.Api";
    public string Audience { get; set; } = "BFCAI.Client";
    /// <summary>At least 32 characters. Set it with user-secrets / environment variables.</summary>
    public string Key { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 7;
}
