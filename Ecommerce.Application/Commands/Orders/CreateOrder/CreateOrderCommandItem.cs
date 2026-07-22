namespace Ecommerce.Application.Commands.Orders.CreateOrder;

public class CreateOrderCommandItem
{
    public Guid IdProduct { get; set; } = Guid.Empty;
    public int Quantity { get; set; }
}
