using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class OrderItem : BaseEntity
{
    public OrderItem() { }

    public OrderItem(Guid idProduct, int quantity)
    {
        IdProduct = idProduct;
        Quantity = quantity;
    }

    [Required]
    public Guid IdProduct { get; set; }

    [Required]
    public Guid IdOrder { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal Price { get; set; }
    public OrderItemReview Review { get; set; }
    public Product Product { get; set; }

    public void SetItemPrice(decimal price) => Price = price;
}
