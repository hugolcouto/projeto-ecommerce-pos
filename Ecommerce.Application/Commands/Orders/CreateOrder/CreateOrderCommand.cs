using Ecommerce.Application.Common;

namespace Ecommerce.Application.Commands.Orders.CreateOrder;

public class CreateOrderCommand
{
    public Guid IdCustomer { get; set; } = Guid.Empty;
    public List<CreateOrderCommandItem> Items { get; set; } = [];
    public Guid IdDeliveryAddress { get; set; } = Guid.Empty;
}

public class CreateOrderCommandItem
{
    public Guid IdProduct { get; set; } = Guid.Empty;
    public int Quantity { get; set; }
}

public class CreateOrderCommandHandler : IHandler<CreateOrderCommand, ResultViewModel<Guid>>
{
    public Task<ResultViewModel<Guid>> HandleAsync(CreateOrderCommand request)
    {
        throw new NotImplementedException();
    }
}