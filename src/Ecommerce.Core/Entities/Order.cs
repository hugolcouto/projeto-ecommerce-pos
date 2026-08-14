using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;
using Ecommerce.Core.Enums;

namespace Ecommerce.Core.Entities;

public class Order : BaseEntity
{
    public Order() { }

    public Order(
        Guid idCustomer,
        Guid deliveryAddressId,
        decimal shippingPrice,
        List<OrderItem> items
    )
    {
        IdCustomer = idCustomer;
        Status = OrderStatus.Created;
        DeliveryAddressId = deliveryAddressId;
        ShippingPrice = shippingPrice;
        Items = items;
        Updates = [];
    }

    public DateTimeOffset? ConfirmDate { get; private set; }
    public DateTimeOffset? ShippingDate { get; private set; }

    [Required]
    [JsonInclude]
    public OrderStatus Status { get; private set; }

    [Required]
    public Guid DeliveryAddressId { get; init; }

    [Required]
    public CustomerAddress DeliveryAddress { get; init; }

    [Required]
    public decimal ShippingPrice { get; private set; }

    [Required]
    public decimal TotalPrice { get; set; }
    public List<OrderItem> Items { get; }

    public Customer Customer { get; }

    public Guid IdCustomer { get; init; }
    public List<OrderUpdate> Updates { get; } = [];
    public string? IdExternalOrder { get; set; }
    public string? PaymentUrl { get; set; }

    public void MarkAsPaymentPending()
    {
        if (Status != OrderStatus.Created)
        {
            Console.WriteLine("[ORDER] Order is in invalid state for payment pending");
            throw new Exception("Order is in invalid state for payment pending");
        }

        Status = OrderStatus.PaymentPending;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkAsPaymentExpired()
    {
        if (Status != OrderStatus.PaymentPending)
        {
            Console.WriteLine("Order is in invalid state for payment expired");
            throw new Exception("Order is in invalid state for payment expired");
        }

        Status = OrderStatus.PaymentExpired;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SetShippingCost(decimal shippingCost)
    {
        if (Status is not OrderStatus.Created)
            throw new InvalidOperationException("Order is invalid state for shipping");

        ShippingPrice = shippingCost;
    }

    public void SetTotalProductPrice(decimal totalPrice) => TotalPrice = totalPrice;
}
