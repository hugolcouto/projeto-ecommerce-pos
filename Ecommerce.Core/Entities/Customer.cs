using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class Customer(string fullName, string email, string phoneNumber, string document, DateTime birthDate, List<CustomerAddress> addresses)
    : BaseEntity
{
    [Required, MaxLength(60)] public string FullName { get; set; } = fullName;
    [Required, MaxLength(20)] public string Email { get; set; } = email;
    [Required, MaxLength(14)] public string PhoneNumber { get; set; } = phoneNumber;

    [Required, MaxLength(15)]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$",
        ErrorMessage = "O campo Documento deve estar no formato ###.###.###-##")]
    public string Document { get; set; } = document;

    [Required] public DateTime BirthDate { get; set; } = birthDate;

    public List<CustomerAddress> Addresses { get; set; } = addresses;
}