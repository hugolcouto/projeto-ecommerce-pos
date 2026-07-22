using System.ComponentModel.DataAnnotations;
using System.Data;
using Ecommerce.Core.Enums;

namespace Ecommerce.Core.Entities;

public class Order : BaseEntity
{
    public Order() { }

    public Order(
        Guid idCustomer,
        Guid deliveryAddressId,
        decimal shippingPrice,
        decimal totalPrice,
        List<OrderItem> items
    )
    {
        IdCustomer = idCustomer;
        Status = OrderStatus.Created;
        DeliveryAddressId = deliveryAddressId;
        ShippingPrice = shippingPrice;
        TotalPrice = totalPrice;
        Items = items;
        Updates = [];
    }

    [Required]
    public Guid IdCostumer { get; private set; }
    public DateTimeOffset? ConfirmDate { get; private set; }
    public DateTimeOffset? ShippingDate { get; private set; }

    [Required]
    public OrderStatus Status { get; private set; }

    [Required]
    public Guid DeliveryAddressId { get; private set; }

    [Required]
    public CustomerAddress DeliveryAddress { get; private set; }

    [Required]
    public decimal ShippingPrice { get; private set; }

    [Required]
    public decimal TotalPrice { get; private set; }
    public List<OrderItem> Items { get; private set; }

    public Customer Customer { get; private set; }

    [Required]
    public Guid IdCustomer { get; private set; }
    public List<OrderUpdate> Updates { get; private set; } = [];

    public void MarkAsConfirmed()
    {
        if (Status != OrderStatus.Created)
        {
            Console.WriteLine($"[Order] Order is in invalid state for confirmation");
            throw new Exception("Order is in invalid state for confirmation");
        }

        Status = OrderStatus.Confirmed;
    }
}
