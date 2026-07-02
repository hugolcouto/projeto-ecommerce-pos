namespace Ecommerce.Application.Commands.ProductsCommands.Customers.CreateCostumer;

public class CreateCustomerCommand
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}