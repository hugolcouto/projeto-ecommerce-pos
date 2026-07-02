using Ecommerce.Application.Common;

namespace Ecommerce.Application.Commands.ProductsCommands.Customers.CreateCostumerAddresses;

public class CreateCustomerAddressCommandHandler : IHandler<CreateCustomerAddressCommand, ResultViewModel<Guid>>
{
    public Task<ResultViewModel<Guid>> HandleAsync(CreateCustomerAddressCommand request)
    {
        throw new NotImplementedException();
    }
}