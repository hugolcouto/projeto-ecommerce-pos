using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class ProductCategory : BaseEntity
{
    public ProductCategory() { }

    public ProductCategory(string title, string subCategory)
    {
        Title = title;
        SubCategory = subCategory;
    }

    [Required, MaxLength(20)]
    public string Title { get; set; }

    [Required]
    public string SubCategory { get; set; }
    public List<Product> Products { get; set; }
}
