using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class Product : BaseEntity
{
    public Product() { }

    public Product(
        string title,
        string description,
        decimal price,
        string brand,
        int quantity,
        Guid idCategory
    )
    {
        Title = title;
        Description = description;
        Price = price;
        Brand = brand;
        Quantity = quantity;
        IdCategory = idCategory;
    }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public string Brand { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public Guid IdCategory { get; set; }

    public List<OrderItemReview> Reviews { get; set; } = [];

    [Required]
    public ProductCategory Category { get; set; }
    public List<ProductImage> Images { get; set; } = [];
}
