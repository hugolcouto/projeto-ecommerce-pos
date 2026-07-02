using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class ProductCategory(string title, string subCategory) : BaseEntity
{
    [Required, MaxLength(20)] public string Title { get; set; } = title;
    [Required] public string SubCategory { get; set; } = subCategory;
}