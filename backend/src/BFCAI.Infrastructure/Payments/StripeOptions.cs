namespace BFCAI.Infrastructure.Payments;

public class StripeOptions
{
    public const string SectionName = "Stripe";

    /// <summary>Set with user-secrets / environment variables - never commit it.</summary>
    public string SecretKey { get; set; } = string.Empty;
    public string Currency { get; set; } = "usd";
}
