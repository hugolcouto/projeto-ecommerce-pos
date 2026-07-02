namespace Ecommerce.Application.Commands.ProductsCommands.Customers.CreateCostumerAddresses;

public class CreateCustomerAddressCommand
{
    public Guid IdCustomer { get; set; }
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
}