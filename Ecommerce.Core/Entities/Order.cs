using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.Enums;

namespace Ecommerce.Core.Entities;

public class Order(
    Guid idCostumer,
    DateTime? confirmDate,
    DateTime? shippingDate,
    OrderStatus orderStatus,
    Guid deliveryAddressId,
    CustomerAddress deliveryAddress,
    decimal shippingCost,
    decimal totalPrice,
    List<OrderItem> orderItems)
    : BaseEntity
{
    [Required] public Guid IdCostumer { get; set; } = idCostumer;
    public DateTime? ConfirmDate { get; set; } = confirmDate;
    public DateTime? ShippingDate { get; set; } = shippingDate;
    [Required] public OrderStatus OrderStatus { get; set; } = orderStatus;
    [Required] public Guid DeliveryAddressId { get; set; } = deliveryAddressId;
    [Required] public CustomerAddress DeliveryAddress { get; set; } = deliveryAddress;
    [Required] public decimal ShippingCost { get; set; } = shippingCost;
    [Required] public decimal TotalPrice { get; set; } = totalPrice;
    public List<OrderItem> OrderItems { get; set; } = orderItems;
}