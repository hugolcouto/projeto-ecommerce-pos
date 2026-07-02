using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class CustomerAddress(
    string addressLine1,
    string addressLine2,
    string zipCode,
    string street,
    string district,
    string city,
    string state,
    string recipientName)
    : BaseEntity
{
    [Required] public string AddressLine1 { get; set; } = addressLine1;
    public string? AddressLine2 { get; set; } = addressLine2;

    [Required]
    [RegularExpression(@"^\d{5}-d{3}$", ErrorMessage = "O campo CEP deve estar no formato #####-###")]
    public string ZipCode { get; set; } = zipCode;

    [Required] public string Street { get; set; } = street;
    [Required] public string District { get; set; } = district;
    [Required] public string City { get; set; } = city;
    [Required] public string State { get; set; } = state;
    [Required] public string RecipientName { get; set; } = recipientName;
}
