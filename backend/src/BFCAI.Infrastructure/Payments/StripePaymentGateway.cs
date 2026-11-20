using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace BFCAI.Infrastructure.Payments;

/// <summary>Stripe Checkout, same session settings as the legacy StripeGetWay.</summary>
public class StripePaymentGateway(IOptions<StripeOptions> options, ILogger<StripePaymentGateway> logger) : IPaymentGateway
{
    private readonly StripeOptions _options = options.Value;
    private StripeClient Client => new(_options.SecretKey);

    public async Task<CheckoutSession> CreateCheckoutSessionAsync(IEnumerable<CheckoutLine> lines, string successUrl, string cancelUrl)
    {
        if (string.IsNullOrWhiteSpace(_options.SecretKey))
        {
            logger.LogError("Stripe:SecretKey is not configured (set it with user-secrets or an environment variable)");
            throw new BadRequestException("Online payment is not available right now.");
        }

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

        try
        {
            var session = await new SessionService(Client).CreateAsync(sessionOptions);
            return new CheckoutSession(session.Id, session.Url);
        }
        catch (StripeException ex)
        {
            logger.LogError(ex, "Stripe checkout session could not be created");
            throw new BadRequestException("Your cart is empty or failed to start payment session.");
        }
    }

    public async Task<bool> IsSessionPaidAsync(string sessionId)
    {
        if (string.IsNullOrWhiteSpace(_options.SecretKey)) return false;

        try
        {
            var session = await new SessionService(Client).GetAsync(sessionId);
            return session.PaymentStatus == "paid";
        }
        catch (StripeException ex)
        {
            logger.LogWarning(ex, "Stripe session {SessionId} could not be read", sessionId);
            return false;
        }
    }
}
