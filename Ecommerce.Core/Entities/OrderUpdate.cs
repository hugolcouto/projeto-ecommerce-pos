using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class OrderUpdate(string description, Guid idOrder) : BaseEntity
{
    [Required] public string Description { get; set; } = description;
    [Required] public Guid IdOrder { get; set; } = idOrder;
}