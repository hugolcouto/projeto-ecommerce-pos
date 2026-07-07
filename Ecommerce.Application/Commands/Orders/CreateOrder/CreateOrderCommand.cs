namespace Ecommerce.Application.Commands.Orders.CreateOrder;

public class CreateOrderCommand
{
    public Guid IdCustomer { get; set; } = Guid.Empty;
    public List<CreateOrderCommandItem> Items { get; set; } = [];
    public Guid IdDeliveryAddress { get; set; } = Guid.Empty;
}
