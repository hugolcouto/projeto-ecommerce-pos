using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class OrderItemReview : BaseEntity
{
    public OrderItemReview() { }

    public OrderItemReview(
        Guid idOrderItem,
        Guid idCustomer,
        string title,
        string description,
        int score
    )
    {
        IdOrderItem = idOrderItem;
        IdCustomer = idCustomer;
        Title = title;
        Description = description;
        Score = score;
    }

    [Required]
    public Guid IdOrderItem { get; set; }

    [Required]
    public Guid IdCustomer { get; set; }

    [Required, MaxLength(30)]
    public string Title { get; set; }

    [Required, MaxLength(600)]
    public string Description { get; set; }

    [Required]
    [Range(0, 5)]
    public int Score { get; set; }

    public Customer Customer { get; set; }
    public OrderItem OrderItem { get; set; }
}
