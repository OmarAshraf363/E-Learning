using BFCAI.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace BFCAI.Infrastructure.Payments;

/// <summary>Stripe Checkout, same session settings as the legacy StripeGetWay.</summary>
public class StripePaymentGateway(IOptions<StripeOptions> options) : IPaymentGateway
{
    private readonly StripeOptions _options = options.Value;
    private StripeClient Client => new(_options.SecretKey);

    public async Task<CheckoutSession> CreateCheckoutSessionAsync(IEnumerable<CheckoutLine> lines, string successUrl, string cancelUrl)
    {
        var sessionOptions = new SessionCreateOptions
        {
            Mode = "payment",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            LineItems = lines.Select(line => new SessionLineItemOptions
            {
                Quantity = line.Quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = _options.Currency,
                    UnitAmount = (long)(line.UnitPrice * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions { Name = line.Name },
                },
            }).ToList(),
        };

        var session = await new SessionService(Client).CreateAsync(sessionOptions);
        return new CheckoutSession(session.Id, session.Url);
    }

    public async Task<bool> IsSessionPaidAsync(string sessionId)
    {
        try
        {
            var session = await new SessionService(Client).GetAsync(sessionId);
            return session.PaymentStatus == "paid";
        }
        catch (StripeException)
        {
            return false;
        }
    }
}
