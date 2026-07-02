using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class OrderItem(Guid productId, Guid idOrder, int quantity, decimal price)
    : BaseEntity
{
    [Required] public Guid ProductId { get; set; } = productId;
    [Required] public Guid IdOrder { get; set; } = idOrder;
    [Required] public int Quantity { get; set; } = quantity;
    [Required] public decimal Price { get; set; } = price;
}