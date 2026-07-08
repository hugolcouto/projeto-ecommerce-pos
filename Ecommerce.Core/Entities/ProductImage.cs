using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class ProductImage : BaseEntity
{
    public ProductImage() { }

    public ProductImage(string identifier, string path, bool isVisible, Guid idProduct)
    {
        Identifier = identifier;
        Path = path;
        IsVisible = isVisible;
        IdProduct = idProduct;
    }

    [Required]
    public string Identifier { get; set; }

    [Required]
    public string Path { get; set; }

    [Required]
    public bool IsVisible { get; set; }

    [Required]
    public Guid IdProduct { get; set; }
}
