using System.Collections.Concurrent;
using BFCAI.Application.Common.Interfaces;

namespace BFCAI.Api.IntegrationTests;

/// <summary>Replaces Stripe in tests. Call MarkPaid(sessionId) to simulate a completed payment.</summary>
public class FakePaymentGateway : IPaymentGateway
{
    public readonly ConcurrentDictionary<string, (List<CheckoutLine> Lines, bool Paid)> Sessions = new();

    public Task<CheckoutSession> CreateCheckoutSessionAsync(IEnumerable<CheckoutLine> lines, string successUrl, string cancelUrl)
    {
        var id = $"cs_test_{Guid.NewGuid():N}";
        Sessions[id] = (lines.ToList(), false);
        return Task.FromResult(new CheckoutSession(id, $"https://checkout.stripe.test/{id}"));
    }

    public Task<bool> IsSessionPaidAsync(string sessionId)
        => Task.FromResult(Sessions.TryGetValue(sessionId, out var s) && s.Paid);

    public void MarkPaid(string sessionId) => Sessions[sessionId] = Sessions[sessionId] with { Paid = true };
}
