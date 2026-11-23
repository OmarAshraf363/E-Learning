using BFCAI.Api.Infrastructure;
using BFCAI.Application.Features.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BFCAI.Api.Controllers.Public;

/// <summary>Shopping cart and Stripe checkout of the logged-in user (legacy Cart + checkout controllers).</summary>
[ApiController]
[Route("api/cart")]
[Authorize]
public class CartController(ICartService cart, ICheckoutService checkout, IOptions<ClientOptions> client) : ControllerBase
{
    [HttpGet] public Task<CartDto> Get() => cart.GetAsync();
    [HttpGet("summary")] public Task<CartSummaryDto> Summary() => cart.GetSummaryAsync();
    [HttpPost("items")] public Task<CartDto> Add(AddToCartRequest request) => cart.AddAsync(request);
    [HttpDelete("items/{id:int}")] public Task<CartDto> Remove(int id) => cart.RemoveAsync(id);

    /// <summary>Returns the Stripe Checkout URL; the browser is redirected there.</summary>
    [HttpPost("checkout")]
    public Task<CheckoutStartDto> Checkout()
    {
        var baseUrl = client.Value.BaseUrl.TrimEnd('/');
        return checkout.StartAsync(
            $"{baseUrl}/cart/checkout/success?session_id={{CHECKOUT_SESSION_ID}}",
            $"{baseUrl}/cart/checkout/cancel");
    }

    [HttpPost("checkout/confirm")]
    public Task<CheckoutResultDto> Confirm([FromQuery] string sessionId) => checkout.ConfirmAsync(sessionId);
}
