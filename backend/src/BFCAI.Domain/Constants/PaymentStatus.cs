namespace BFCAI.Domain.Constants;

/// <summary>Order payment status values (same values as the legacy StaticData).</summary>
public static class PaymentStatus
{
    public const string SuccessConfirmed = "Success-Confirmed";
    public const string Refunded = "Payment-Refunded";
    public const string InProcess = "In-Process";
    public const string Pending = "Pending";
}

/// <summary>Order.OrderStatus byte values used by the legacy code.</summary>
public static class OrderStatus
{
    public const byte Pending = 0;
    public const byte Paid = 1;
}
