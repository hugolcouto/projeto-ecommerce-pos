namespace Ecommerce.Core.Enums;

public enum OrderStatus
{
    Created = 1,
    PaymentPending,
    Confirmed,
    Picking,
    Shipped,
    Delivered,
    Cancelled,
    PaymentExpired,
}
