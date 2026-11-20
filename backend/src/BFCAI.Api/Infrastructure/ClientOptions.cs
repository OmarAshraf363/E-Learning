namespace BFCAI.Api.Infrastructure;

/// <summary>The Angular app (used to build the Stripe success / cancel URLs).</summary>
public class ClientOptions
{
    public const string SectionName = "Client";
    public string BaseUrl { get; set; } = "http://localhost:4200";
}
