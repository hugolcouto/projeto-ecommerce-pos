using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.Commands.ProductsCommands.Customers.CreateCostumer;

// TODO: Adicionar validação
public class CreateCustomerCommand
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    [RegularExpression(
        @"^\d{3}\.\d{3}\.\d{3}\-\d{2}$",
        ErrorMessage = "CPF inválido. Formato: 000.000.000-00"
    )]
    public string Document { get; set; } = string.Empty;
    public DateTimeOffset BirthDate { get; set; } = DateTimeOffset.UtcNow;
}
