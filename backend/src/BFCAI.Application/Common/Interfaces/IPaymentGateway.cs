namespace BFCAI.Application.Common.Interfaces;

public record CheckoutLine(string Name, decimal UnitPrice, int Quantity);

public record CheckoutSession(string Id, string Url);

/// <summary>Payment provider (Stripe).</summary>
public interface IPaymentGateway
{
    Task<CheckoutSession> CreateCheckoutSessionAsync(IEnumerable<CheckoutLine> lines, string successUrl, string cancelUrl);

    /// <summary>True when the checkout session exists and is paid.</summary>
    Task<bool> IsSessionPaidAsync(string sessionId);
}
