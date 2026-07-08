using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class CustomerAddress : BaseEntity
{
    public CustomerAddress() { }

    public CustomerAddress(
        Guid idCustomer,
        string recipientName,
        string addressLine1,
        string addressLine2,
        string zipCode,
        string street,
        string district,
        string city,
        string state
    )
    {
        IdCustomer = idCustomer;
        RecipientName = recipientName;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        ZipCode = zipCode;
        Street = street;
        District = district;
        City = city;
        State = state;
    }

    [Required]
    public Guid IdCustomer { get; set; }

    [Required]
    public string AddressLine1 { get; set; }

    [Required]
    public string AddressLine2 { get; set; }

    [Required]
    public string ZipCode { get; set; }

    [Required]
    public string Street { get; set; }

    [Required]
    public string District { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string State { get; set; }

    [Required]
    public string RecipientName { get; set; }
}
