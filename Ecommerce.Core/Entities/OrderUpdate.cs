using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class OrderUpdate : BaseEntity
{
    public OrderUpdate() { }

    public OrderUpdate(string description, Guid idOrder)
    {
        Description = description;
        IdOrder = idOrder;
    }

    [Required]
    public string Description { get; set; }

    [Required]
    public Guid IdOrder { get; set; }
}
