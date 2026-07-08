using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class Customer : BaseEntity
{
    public Customer() { }

    public Customer(
        string fullName,
        string email,
        string phoneNumber,
        string document,
        DateTimeOffset birthDate
    )
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Document = document;
        BirthDate = birthDate;
    }

    [Required, MaxLength(60)]
    public string FullName { get; set; }

    [Required, MaxLength(20)]
    public string Email { get; set; }

    [Required, MaxLength(14)]
    public string PhoneNumber { get; set; }

    [Required, MaxLength(15)]
    [RegularExpression(
        @"^\d{3}\.\d{3}\.\d{3}-\d{2}$",
        ErrorMessage = "O campo Documento deve estar no formato ###.###.###-##"
    )]
    public string Document { get; set; }

    [Required]
    public DateTimeOffset BirthDate { get; set; }

    public List<CustomerAddress> Addresses { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
    public List<OrderItemReview> Reviews { get; set; } = [];
}
