using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class ProductImage : BaseEntity
{
    public ProductImage() { }

    public ProductImage(bool isVisible, Guid idProduct)
    {
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

    public void ConfigureIdentifier(string fileExtension)
    {
        Identifier = Id.ToString();
        Path = $"{IdProduct}/{Id}.{fileExtension}";
    }
}
