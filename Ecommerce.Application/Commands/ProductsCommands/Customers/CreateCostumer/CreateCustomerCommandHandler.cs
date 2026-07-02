using Ecommerce.Application.Common;

namespace Ecommerce.Application.Commands.ProductsCommands.Customers.CreateCostumer;

public class CreateCustomerCommandHandler : IHandler<CreateCustomerCommand, ResultViewModel<Guid>>
{
    public Task<ResultViewModel<Guid>> HandleAsync(CreateCustomerCommand request)
    {
        throw new NotImplementedException();
    }
}