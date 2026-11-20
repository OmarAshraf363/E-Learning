using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Domain.Constants;
using BFCAI.Domain.Entities;
using CartEntity = BFCAI.Domain.Entities.Cart;

namespace BFCAI.Application.Features.Cart;

/// <summary>Stripe checkout (legacy CartController.Pay + checkoutController + PaymentService).</summary>
public interface ICheckoutService
{
    /// <summary>Creates / refreshes the pending order from the cart and returns the Stripe Checkout URL.</summary>
    Task<CheckoutStartDto> StartAsync(string successUrl, string cancelUrl);

    /// <summary>Called by the success page: verifies the payment with Stripe, then enrolls the student.</summary>
    Task<CheckoutResultDto> ConfirmAsync(string sessionId);
}

public class CheckoutService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IPaymentGateway gateway) : ICheckoutService
{
    private IGenericRepository<Order> Orders => unitOfWork.Repository<Order>();

    public async Task<CheckoutStartDto> StartAsync(string successUrl, string cancelUrl)
    {
        var userId = currentUser.RequiredUserId;

        var cartItems = await unitOfWork.Repository<CartItems>().GetAllAsync(
            i => i.Cart!.UserId == userId && i.CourseId != null, includes: i => i.Course);
        if (cartItems.Count == 0)
            throw new BadRequestException("Your cart is empty or failed to start payment session.");

        // Legacy: one pending order per user (OrderStatus 0); its items are rebuilt from the cart.
        var order = await Orders.GetOneAsync(o => o.AppUserId == userId && o.OrderStatus == OrderStatus.Pending,
                        tracked: true, o => o.OrderItems);
        if (order is null)
        {
            order = new Order { AppUserId = userId, OrderStatus = OrderStatus.Pending, OrderDate = DateTime.Now };
            await Orders.AddAsync(order);
        }
        else
        {
            unitOfWork.Repository<OrderItems>().DeleteRange(order.OrderItems);
        }

        order.OrderItems = cartItems.Select(i => new OrderItems
        {
            CourseId = i.CourseId,
            Quantity = i.Quantity,
            ListPrice = i.Course?.Price,
            TotalPrice = i.Quantity * i.Course?.Price,
        }).ToList();

        var session = await gateway.CreateCheckoutSessionAsync(
            cartItems.Select(i => new CheckoutLine(i.Course?.CourseName ?? "Course", i.Price ?? 0, i.Quantity ?? 1)),
            successUrl, cancelUrl);

        // Technical fix: legacy lost this value when the order already existed (untracked entity).
        order.StripeChargeId = session.Id;
        await unitOfWork.SaveChangesAsync();

        return new CheckoutStartDto(session.Url);
    }

    public async Task<CheckoutResultDto> ConfirmAsync(string sessionId)
    {
        var userId = currentUser.RequiredUserId;
        var order = await Orders.GetOneAsync(o => o.AppUserId == userId && o.StripeChargeId == sessionId,
                        tracked: true, o => o.OrderItems)
                    ?? throw new NotFoundException("Order", sessionId);

        // Refreshing the success page must not enroll twice.
        if (order.OrderStatus == OrderStatus.Paid)
            return ToResult(order);

        // Technical / security fix: legacy enrolled the student without checking the payment.
        if (!await gateway.IsSessionPaidAsync(sessionId))
            throw new BadRequestException("The payment was not completed.");

        order.OrderStatus = OrderStatus.Paid;
        order.RequiredDate = DateTime.Now;
        order.ShippedDate = DateTime.Now;
        order.PaymentStatus = PaymentStatus.InProcess; // same value the legacy app stored

        foreach (var item in order.OrderItems)
        {
            item.TotalPrice = item.ListPrice * item.Quantity;
            await unitOfWork.Repository<Enrollment>().AddAsync(new Enrollment { StudentId = userId, CourseID = item.CourseId });
        }

        // Legacy: the cart and its items are deleted after payment.
        var cart = await unitOfWork.Repository<CartEntity>().GetOneAsync(c => c.UserId == userId, tracked: true, c => c.Items);
        if (cart is not null)
        {
            unitOfWork.Repository<CartItems>().DeleteRange(cart.Items);
            unitOfWork.Repository<CartEntity>().Delete(cart);
        }

        await unitOfWork.SaveChangesAsync();
        return ToResult(order);
    }

    private static CheckoutResultDto ToResult(Order order)
        => new(order.Id, order.OrderItems.Sum(i => i.TotalPrice ?? 0), order.ShippedDate ?? order.OrderDate);
}
