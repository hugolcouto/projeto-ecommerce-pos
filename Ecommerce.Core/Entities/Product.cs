using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class Product(string title, string description, decimal price, string brand, int quantity, Guid idCategory)
    : BaseEntity
{
    [Required] public string Title { get; set; } = title;
    [Required] public string Description { get; set; } = description;
    [Required] public decimal Price { get; set; } = price;
    [Required] public string Brand { get; set; } = brand;
    [Required] public int Quantity { get; set; } = quantity;
    [Required] public Guid IdCategory { get; set; } = idCategory;
}