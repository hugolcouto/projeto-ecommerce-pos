using Ecommerce.Application.Common;

namespace Ecommerce.Application.Commands.Orders.CreateOrder;

public class CreateOrderCommandHandler : IHandler<CreateOrderCommand, ResultViewModel<Guid>>
{
    public Task<ResultViewModel<Guid>> HandleAsync(CreateOrderCommand request)
    {
        throw new NotImplementedException();
    }
}