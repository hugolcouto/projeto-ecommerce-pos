using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class OrderProductReview(Guid idOrderItem, Guid idCustomer, string title, string description, int score)
{
    [Required] public Guid IdOrderItem { get; set; } = idOrderItem;
    [Required] public Guid IdCustomer { get; set; } = idCustomer;
    [Required, MaxLength(30)] public string Title { get; set; } = title;
    [Required, MaxLength(600)] public string Description { get; set; } = description;
    [Required] [Range(0, 5)] public int Score { get; set; } = score;
}