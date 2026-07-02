using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class ProductImage(string identifier, string path, bool isVisible) : BaseEntity
{
    [Required] public string Identifier { get; set; } = identifier;
    [Required] public string Path { get; set; } = path;
    [Required] public bool IsVisible { get; set; } = isVisible;
}